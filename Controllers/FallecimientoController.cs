using membresias.be.Models;
using membresias.be.Models.Dtos;
using membresias.be.Services;
using Microsoft.AspNetCore.Mvc;

namespace membresias.be.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FallecimientoController : ControllerBase
    {
        private readonly FallecimientoService _fallecimientoService; 

        public FallecimientoController(FallecimientoService fallecimientoService)
        {
            _fallecimientoService = fallecimientoService; 
        }

        [HttpPost("CreateFallecimiento")]
        public async Task<bool> CreateFallecimientoAsync(Fallecimiento fallecimiento)
        {
            return await _fallecimientoService.CreateFallecimiento(fallecimiento); 
        }

        [HttpGet("GetFallecimientos")]
        public async Task<IEnumerable<FallecimientoDto>> GetFallecimientosAsync()
        {
            return await _fallecimientoService.GetFallecimientos(); 
        }
    }
}
