using AutoMapper;
using membresias.be.Db;
using membresias.be.Enumerations;
using membresias.be.Exceptions;
using membresias.be.Models;
using membresias.be.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace membresias.be.Services
{
    public class FallecimientoService
    {
        private readonly MembresiasDbContext _dbContext;
        private readonly ILogger<FallecimientoService> _logger;
        private readonly IMapper _mapper;

        public FallecimientoService(MembresiasDbContext dbContext,
            ILogger<FallecimientoService> logger, 
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper; 
        }

        public async Task<bool> CreateFallecimiento(Fallecimiento fallecimiento)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(CreateFallecimiento)}.");

                if (fallecimiento.FechaFallecimiento.Date > DateTime.UtcNow.Date)
                    throw new ValidationException("Fallecimientos", "No se pueden registrar fallecimientos con fecha mayor al día de hoy."); 
                
                var fallecimientoDuplicate = await _dbContext.Fallecimientos
                    .Where(f => !f.IsDeleted
                        && f.IdMiembro == fallecimiento.IdMiembro)
                    .FirstOrDefaultAsync();

                if (fallecimientoDuplicate != null)
                    throw new ValidationException("Fallecimientos", $"Ya existe un fallecimiento registrado para el miembro {fallecimiento.IdMiembro}."); 

                await _dbContext.Fallecimientos.AddAsync(fallecimiento);

                var miembroFallecido = await _dbContext.Miembros
                    .Where(m => m.IdMiembro == fallecimiento.IdMiembro)
                    .FirstAsync();

                miembroFallecido.MiembroEstatusCodigo = MiembroEstatus.Fallecido.Codigo;

                var miembros = await _dbContext.Miembros
                    .Where(m => !m.IsDeleted
                        && m.MiembroEstatusCodigo!.Equals(MiembroEstatus.Activo.Codigo)
                        && m.MembresiaCodigo.Equals(Membresia.Socio.Codigo)
                        && m.IdMiembro != miembroFallecido.IdMiembro)
                    .ToListAsync();

                var tarifas = await _dbContext.Tarifas
                    .Where(t => !t.IsDeleted
                        && t.ConceptoCodigo.Equals(Concepto.AyudaMutua.Codigo))
                    .ToListAsync();

                var cargosToCreate = new List<Cargo>(); 
                if (miembros.Count > 0)
                {
                    foreach(var miembro in miembros)
                    {
                        var monto = tarifas.First(t => t.MembresiaCodigo.Equals(miembro.MembresiaCodigo)).Monto;

                        var cargo = new Cargo()
                        {
                            Descripcion = "AYUDA MUTUA",
                            Monto = monto,
                            FechaCargo = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6)),
                            IdMiembro = miembro.IdMiembro,
                            ConceptoCodigo = Concepto.AyudaMutua.Codigo
                        }; 

                        cargosToCreate.Add(cargo); 
                    }

                    await _dbContext.AddRangeAsync(cargosToCreate); 
                }

                var result = await _dbContext.SaveChangesAsync() > 0;
                return result; 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(CreateFallecimiento)}.");
            }
        }

        public async Task<IEnumerable<FallecimientoDto>> GetFallecimientos()
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(GetFallecimientos)}.");

                var fallecimientos = await _dbContext.Fallecimientos
                    .Where(f => !f.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation($"Fallecimientos: {fallecimientos.Count}"); 

                return _mapper.Map<IEnumerable<FallecimientoDto>>(fallecimientos); 
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(GetFallecimientos)}."); 
            }
        }
    }
}
