using membresias.be.Db;
using membresias.be.Models;
using membresias.be.Models.Dtos;
using membresias.be.Services;
using Microsoft.AspNetCore.Mvc;

namespace membresias.be.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarifaController : ControllerBase
    {
        public TarifaService _tarifaService; 
        public TarifaController(TarifaService tarifaService)
        {
            _tarifaService = tarifaService; 
        }

        [HttpPost("CreateTarifa")]
        public async Task<bool> CreateTarifaAsync(Tarifa tarifa)
        {
            return await _tarifaService.CreateTarifa(tarifa); 
        }

        [HttpGet("GetTarifas")]
        public async Task<IEnumerable<TarifaDto>> GetTarifasAsync()
        {
            return await _tarifaService.GetTarifas(); 
        }

        [HttpGet("GetTarifaById/{tarifaId}")]
        public async Task<TarifaDto> GetTarifaByIdAsync(int tarifaId)
        {
            return await _tarifaService.GetTarifaById(tarifaId); 
        }

        [HttpPut("UpdateTarifa/{tarifaId}")]
        public async Task<bool> UpdateTarifaAsync(int tarifaId, Tarifa tarifa)
        {
            return await _tarifaService.UpdateTarifa(tarifaId, tarifa); 
        }

        [HttpDelete("DeleteTarifa/{tarifaId}")]
        public async Task<bool> DeleteTarifaAsync(int tarifaId)
        {
            return await _tarifaService.DeleteTarifa(tarifaId); 
        }
    }
}
