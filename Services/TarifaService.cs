using AutoMapper;
using membresias.be.Db;
using membresias.be.Enumerations;
using membresias.be.Exceptions;
using membresias.be.Models;
using membresias.be.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace membresias.be.Services
{
    public class TarifaService
    {
        private readonly MembresiasDbContext _dbContext;
        private readonly ILogger<TarifaService> _logger;
        private readonly IMapper _mapper;

        public TarifaService(MembresiasDbContext dbContext,
            ILogger<TarifaService> logger,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> CreateTarifa(Tarifa tarifa)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(CreateTarifa)}.");

                var tarifaDuplicate = await _dbContext.Tarifas
                    .Where(t => t.MembresiaCodigo.Equals(tarifa.MembresiaCodigo)
                        && t.ConceptoCodigo.Equals(tarifa.ConceptoCodigo))
                    .FirstOrDefaultAsync();

                bool result;

                // Verifica si ya existe una tarifa con la misma membresía y concepto. 
                // Si ya existe y esta eliminada, reactiva la tarifa y actualiza los datos. 
                // Si no, crea una tarifa nueva. 
                if (tarifaDuplicate != null)
                {
                    if (tarifaDuplicate.IsDeleted)
                    {
                        tarifaDuplicate.IsDeleted = false;
                        tarifaDuplicate.Nombre = tarifa.Nombre;
                        tarifaDuplicate.Monto = tarifa.Monto;
                        tarifaDuplicate.MembresiaCodigo = tarifa.MembresiaCodigo;
                        tarifaDuplicate.ConceptoCodigo = tarifa.ConceptoCodigo;
                        tarifaDuplicate.ModifiedDate = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));

                        result = await _dbContext.SaveChangesAsync() > 0;

                        if (result)
                            _logger.LogInformation($"Se ha reactivado una tarifa existente. Nombre: {tarifa.Nombre}, Monto: {tarifa.Monto}.");

                        return result;
                    }
                    else
                    {
                        throw new ValidationException("Tarifas", $"Ya existe una tarifa para la membresia {Membresia.GetByCode(tarifa.MembresiaCodigo).Nombre}" +
                            $" y concepto {Concepto.GetByCode(tarifa.ConceptoCodigo).Nombre}.");
                    }
                }

                await _dbContext.Tarifas.AddAsync(tarifa);
                result = await _dbContext.SaveChangesAsync() > 0;

                if (result)
                    _logger.LogInformation($"Se ha creado una nueva tarifa. Nombre: {tarifa.Nombre}, Monto: {tarifa.Monto}.");

                return result;
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(CreateTarifa)}.");
            }
        }

        public async Task<IEnumerable<TarifaDto>> GetTarifas()
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(GetTarifas)}.");

                var tarifas = await _dbContext.Tarifas
                    .Where(t => !t.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation($"tarifas: {tarifas.Count}.");

                return _mapper.Map<IEnumerable<TarifaDto>>(tarifas);
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(GetTarifas)}.");
            }
        }

        public async Task<TarifaDto> GetTarifaById(int id)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(GetTarifaById)}.");

                var tarifa = await _dbContext.Tarifas
                    .Where(t => !t.IsDeleted && t.IdTarifa == id)
                    .FirstOrDefaultAsync();

                if (tarifa == null)
                    throw new NotFoundException("Tarifas", id);

                _logger.LogInformation($"Se ha encontrado una tarifa. Nombre: {tarifa.Nombre}, Monto: {tarifa.Monto}.");

                return _mapper.Map<TarifaDto>(tarifa);
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(GetTarifaById)}.");
            }
        }

        public async Task<bool> UpdateTarifa(int id, Tarifa tarifa)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(UpdateTarifa)}.");

                var tarifaDuplicate = await _dbContext.Tarifas
                    .Where(t => t.MembresiaCodigo.Equals(tarifa.MembresiaCodigo)
                        && t.ConceptoCodigo.Equals(tarifa.ConceptoCodigo)
                        && t.IdTarifa != id)
                    .FirstOrDefaultAsync();

                // Verifica si ya existe una tarifa con la misma membresía y concepto.  
                // Si no existe, crea una tarifa nueva. 
                if (tarifaDuplicate != null)
                    throw new ValidationException("Tarifas", $"Ya existe una tarifa para la membresia {Membresia.GetByCode(tarifa.MembresiaCodigo).Nombre}" +
                        $" y concepto {Concepto.GetByCode(tarifa.ConceptoCodigo).Nombre}.");

                var tarifaToUpdate = await _dbContext.Tarifas
                .Where(t => !t.IsDeleted && t.IdTarifa == id)
                .FirstOrDefaultAsync();

                if (tarifaToUpdate == null)
                    throw new ValidationException("Tarifas", "No se encontró la tarifa.");

                tarifaToUpdate.Nombre = tarifa.Nombre;
                tarifaToUpdate.Monto = tarifa.Monto;
                tarifaToUpdate.MembresiaCodigo = tarifa.MembresiaCodigo;
                tarifaToUpdate.ConceptoCodigo = tarifa.ConceptoCodigo;
                tarifaToUpdate.ModifiedDate = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));

                var result = await _dbContext.SaveChangesAsync() > 0;
                return result;
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(UpdateTarifa)}.");
            }
        }

        public async Task<bool> DeleteTarifa(int id)
        {
            try
            {
                _logger.LogInformation($"Iniciando solicitud de {nameof(DeleteTarifa)}.");

                var tarifa = await _dbContext.Tarifas
                    .Where(t => !t.IsDeleted && t.IdTarifa == id)
                    .FirstOrDefaultAsync();

                if (tarifa == null)
                    throw new ValidationException("Tarifas", "No se encontró la tarifa.");

                tarifa.IsDeleted = true;
                tarifa.ModifiedDate = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));

                var result = await _dbContext.SaveChangesAsync() > 0;
                return result;
            }
            finally
            {
                _logger.LogInformation($"Finalizando solicitud de {nameof(DeleteTarifa)}.");
            }
        }
    }
}
