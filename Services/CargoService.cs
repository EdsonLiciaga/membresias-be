using AutoMapper;
using membresias.be.Db;
using membresias.be.Enumerations;
using membresias.be.Exceptions;
using membresias.be.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using System.Globalization;

namespace membresias.be.Services
{
    public class CargoService
    {
        private readonly MembresiasDbContext _dbContext;
        private readonly ILogger<CargoService> _logger;
        private readonly IMapper _mapper;

        public CargoService(MembresiasDbContext dbContext,
            ILogger<CargoService> logger, 
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper; 
        }

        public async Task<bool> CreateCargos()
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(CreateCargos)}.");

                var miembros = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.MiembroEstatusCodigo!.Equals(MiembroEstatus.Activo.Codigo))
                    .ToListAsync();

                if (miembros.Count == 0)
                    throw new ValidationException("Miembros", "No se encontraron miembros activos.");

                var cargos = await _dbContext.Cargos
                    .Where(c => !c.IsDeleted
                        && c.FechaCargo.Month == DateTime.UtcNow.Month
                        && c.ConceptoCodigo.Equals(Concepto.CasaClub.Codigo))
                    .ToListAsync();

                var mes = DateTime.UtcNow.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).ToUpper();

                if (cargos.Count == miembros.Count)
                    throw new ValidationException("Cargos", $"Los pagos del mes {mes} ya fueron enviados a todos los miembros activos anteriormente."); 

                var miembrosConCargos = cargos.Select(c => c.IdMiembro).ToList();

                var miembrosSinCargos = miembros
                    .Where(m => !miembrosConCargos.Contains(m.IdMiembro))
                    .ToList(); 

                var tarifas = await _dbContext.Tarifas
                    .Where(t => !t.IsDeleted
                        && t.ConceptoCodigo.Equals(Concepto.CasaClub.Codigo))
                    .ToListAsync(); 

                var cargosToCreate = new List<Cargo>(); 
                foreach (var miembro in miembrosSinCargos)
                {
                    var descripcion = $"PAGO MES - {mes}";
                    var monto = tarifas.First(t => t.MembresiaCodigo.Equals(miembro.MembresiaCodigo)).Monto;

                    var cargo = new Cargo()
                    {
                        Descripcion = descripcion,
                        Monto = monto,
                        FechaCargo = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6)),
                        IdMiembro = miembro.IdMiembro,
                        ConceptoCodigo = Concepto.CasaClub.Codigo
                    }; 

                    cargosToCreate.Add(cargo); 
                }

                await _dbContext.AddRangeAsync(cargosToCreate);

                _logger.LogInformation($"cargosToCreate: {cargosToCreate.Count}");

                var result = await _dbContext.SaveChangesAsync() > 0;

                if (result)
                    _logger.LogInformation($"Se han creado {cargosToCreate.Count} cargos por el concepto de {Concepto.CasaClub.Codigo}."); 

                return result; 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(CreateCargos)}."); 
            }
        }
    }
}
