using AutoMapper;
using membresias.be.Db;
using membresias.be.Enumerations;
using membresias.be.Exceptions;
using membresias.be.Filters;
using membresias.be.Models;
using membresias.be.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using System.Globalization;

namespace membresias.be.Services
{
    public class MiembroService
    {
        private readonly MembresiasDbContext _dbContext;
        private readonly ILogger<MiembroService> _logger;
        private readonly IMapper _mapper;

        public MiembroService(MembresiasDbContext dbContext,
            ILogger<MiembroService> logger,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> CreateMiembro(Miembro miembro)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(CreateMiembro)}.");

                var edad = CalcularEdad(miembro.FechaNacimiento); 

                if (edad < 40)
                    throw new ValidationException("Miembros", "Solo se admiten miembros de 40 años en adelante.");

                // Verifica si ya hay un miembro con la misma CURP
                var miembroDuplicate = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted 
                        && m.Curp == miembro.Curp)
                    .FirstOrDefaultAsync();
                
                if (miembroDuplicate != null && miembroDuplicate.Curp.Equals(miembro.Curp))
                    throw new ValidationException("Miembros", "La CURP ya está registrada.");

                miembro.Clave = miembro.Nombre.Substring(0, Math.Min(1, miembro.Nombre.Length)).ToUpper()
                    + miembro.PrimerApellido.Substring(0, Math.Min(3, miembro.PrimerApellido.Length)).ToUpper()
                    + miembro.Curp.Substring(miembro.Curp.Length - 4).ToUpper();

                miembro.Edad = edad;
                miembro.FechaIngreso = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));

                await _dbContext.Miembros.AddAsync(miembro);

                var result = await _dbContext.SaveChangesAsync() > 0;

                if (result)
                    _logger.LogInformation($"Se ha creado un nuevo miembro. Nombre: {miembro.Nombre}, " +
                        $"PrimerApellido: {miembro.PrimerApellido}, MembresiaCodigo: {miembro.MembresiaCodigo}.");

                return result;
            }
            finally
            {
                _logger.LogInformation($"Finalizando la solicitud de {nameof(CreateMiembro)}.");
            }
        }

        public async Task<IEnumerable<MiembroDto>> GetMiembros()
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(GetMiembros)}");

                var miembros = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation($"miembros: {miembros.Count}");

                var miembrosDtoList = _mapper.Map<IEnumerable<MiembroDto>>(miembros);

                if (!miembros.Any(m => m.MiembroEstatusCodigo!.Equals(MiembroEstatus.Fallecido.Codigo)))
                    return miembrosDtoList;

                var fallecimientos = await _dbContext.Fallecimientos
                    .Where(f => !f.IsDeleted)
                    .ToListAsync(); 

                foreach (var fallecimiento in fallecimientos)
                {
                    var miembroDto = miembrosDtoList.First(m => m.IdMiembro == fallecimiento.IdMiembro);

                    miembroDto.FechaFallecimiento = fallecimiento.FechaFallecimiento.Date.ToString("yyyy-MM-dd"); 
                }

                return miembrosDtoList; 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(GetMiembros)}"); 
            }
        }

        public async Task<MiembroDto> GetMiembroById(int id)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(GetMiembroById)}");

                var miembro = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.MiembroEstatusCodigo!.Equals(MiembroEstatus.Activo.Codigo)
                        && m.IdMiembro == id)
                    .FirstOrDefaultAsync();

                if (miembro == null)
                    throw new NotFoundException("Miembros", id);

                _logger.LogInformation($"Se ha encontrado un miembro. Nombre: {miembro.Nombre}, PrimerApellido: {miembro.PrimerApellido}.");

                return _mapper.Map<MiembroDto>(miembro); 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(GetMiembroById)}."); 
            }
        }

        public async Task<bool> UpdateMiembro(int id, Miembro miembro)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(UpdateMiembro)}.");

                var edad = CalcularEdad(miembro.FechaNacimiento); 

                if (edad < 40)
                    throw new ValidationException("Miembros", "Solo se admiten miembros de 40 años en adelante.");

                // Verifica si ya hay un miembro con la misma CURP
                var miembroDuplicate = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.Curp == miembro.Curp
                        && m.IdMiembro != id)
                    .FirstOrDefaultAsync();

                if (miembroDuplicate != null && miembroDuplicate.Curp.Equals(miembro.Curp))
                    throw new ValidationException("Miembros", "La CURP ya está registrada.");

                var miembroToUpdate = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.MiembroEstatusCodigo!.Equals(MiembroEstatus.Activo.Codigo)
                        && m.IdMiembro == id)
                    .FirstOrDefaultAsync();

                if (miembroToUpdate == null)
                    throw new ValidationException("Miembros", "No se encontró el miembro.");

                miembroToUpdate.Nombre = miembro.Nombre;
                miembroToUpdate.PrimerApellido = miembro.PrimerApellido;
                miembroToUpdate.SegundoApellido = miembro.SegundoApellido;
                miembroToUpdate.Curp = miembro.Curp; 
                miembroToUpdate.FechaNacimiento = miembro.FechaNacimiento;
                miembroToUpdate.Edad = edad; 
                miembroToUpdate.MembresiaCodigo = miembro.MembresiaCodigo;
                miembroToUpdate.ModifiedDate = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6)); 

                var result = await _dbContext.SaveChangesAsync() > 0;

                if (result)
                    _logger.LogInformation($"Se ha actualizado un miembro. Nombre: {miembro.Nombre}, PrimerApellido: {miembro.PrimerApellido}."); 

                return result; 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(UpdateMiembro)}."); 
            }
        }

        public async Task<bool> DeactivateMiembro(int id)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(DeactivateMiembro)}.");

                var miembro = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.IdMiembro == id
                        && m.MiembroEstatusCodigo!.Equals(MiembroEstatus.Activo.Codigo))
                    .FirstOrDefaultAsync();

                if (miembro == null)
                    throw new ValidationException("Miembros", "No se encontró el miembro.");

                miembro.MiembroEstatusCodigo = MiembroEstatus.Inactivo.Codigo;
                miembro.ModifiedDate = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));

                var result = await _dbContext.SaveChangesAsync() > 0;
                return result; 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(DeactivateMiembro)}."); 
            }
        }

        public async Task<bool> ReactivateMiembro(int id)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(ReactivateMiembro)}.");

                var miembro = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.IdMiembro == id
                        && m.MiembroEstatusCodigo!.Equals(MiembroEstatus.Inactivo.Codigo))
                    .FirstOrDefaultAsync();

                if (miembro == null)
                    throw new ValidationException("Miembros", "No se encontró el miembro.");

                miembro.MiembroEstatusCodigo = MiembroEstatus.Activo.Codigo;
                miembro.ModifiedDate = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));
                miembro.FechaReingreso = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));

                var conceptos = new List<string>()
                {
                    Concepto.Reingreso.Codigo,
                    Concepto.CasaClub.Codigo
                }; 

                var tarifas = await _dbContext.Tarifas  
                    .Where(t => !t.IsDeleted
                        && t.MembresiaCodigo.Equals(miembro.MembresiaCodigo)
                        && conceptos.Contains(t.ConceptoCodigo))
                    .ToListAsync();

                var cargosToCreate = new List<Cargo>(); 
                foreach(var concepto in conceptos)
                {
                    var monto = tarifas.First(t => t.ConceptoCodigo.Equals(concepto)).Monto; 

                    var cargo = new Cargo()
                    {
                        Monto = monto,
                        FechaCargo = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6)),
                        IdMiembro = miembro.IdMiembro,
                        ConceptoCodigo = concepto
                    };

                    cargosToCreate.Add(cargo); 
                }

                var mes = DateTime.UtcNow.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();

                cargosToCreate[0].Descripcion = "REINGRESO";
                cargosToCreate[1].Descripcion = $"PAGO MES - {mes}"; 

                await _dbContext.Cargos.AddRangeAsync(cargosToCreate); 

                var result = await _dbContext.SaveChangesAsync() > 0;
                return result;
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(ReactivateMiembro)}."); 
            }
        }

        public async Task<bool> UpdateMiembroEstatus(int id, string miembroEstatusCodigo)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(UpdateMiembroEstatus)}.");

                if (miembroEstatusCodigo.Equals(MiembroEstatus.Inactivo.Codigo))
                    return await DeactivateMiembro(id);
                else
                    return await ReactivateMiembro(id);          
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(UpdateMiembroEstatus)}.");
            }
        }

        public async Task<bool> DeleteMiembro(int id)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(DeleteMiembro)}.");

                var miembro = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.IdMiembro == id)
                    .FirstOrDefaultAsync();

                if (miembro == null)
                    throw new ValidationException("Miembros", "No se encontró el miembro.");

                miembro.IsDeleted = true;
                miembro.ModifiedDate = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));

                var result = await _dbContext.SaveChangesAsync() > 0;
                return result; 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(DeleteMiembro)}"); 
            }
        }

        public static int CalcularEdad(DateTimeOffset fechaNacimiento)
        {
            var edad = DateTime.UtcNow.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > DateTime.UtcNow.AddYears(-edad))
                edad--;

            return edad; 
        }
    }
}
