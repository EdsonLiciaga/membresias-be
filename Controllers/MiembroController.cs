using membresias.be.Models;
using membresias.be.Models.Dtos;
using membresias.be.Services;
using Microsoft.AspNetCore.Mvc;

namespace membresias.be.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MiembroController : ControllerBase
    {
        private readonly MiembroService _miembroService;

        public MiembroController(MiembroService miembroService)
        {
            _miembroService = miembroService; 
        }

        [HttpPost("CreateMiembro")]
        public async Task<bool> CreateMiembroAsync(Miembro miembro)
        {
            return await _miembroService.CreateMiembro(miembro); 
        }

        [HttpGet("GetMiembros")]
        public async Task<IEnumerable<MiembroDto>> GetMiembrosAsync()
        {
            return await _miembroService.GetMiembros();
        }

        [HttpGet("GetMiembroById/{miembroId}")]
        public async Task<MiembroDto> GetMiembroByIdAsync(int miembroId)
        {
            return await _miembroService.GetMiembroById(miembroId); 
        }

        [HttpPut("UpdateMiembro/{miembroId}")]
        public async Task<bool> UpdateMiembroAsync(int miembroId, Miembro miembro)
        {
            return await _miembroService.UpdateMiembro(miembroId, miembro); 
        }

        [HttpPut("UpdateMiembroEstatus/{miembroId}")]
        public async Task<bool> DeactivateMiembroAsync(int miembroId, string miembroEstatusCodigo)
        {
            return await _miembroService.UpdateMiembroEstatus(miembroId, miembroEstatusCodigo); 
        }

        [HttpDelete("DeleteMiembro/{miembroId}")]
        public async Task<bool> DeleteMiembroAsync(int miembroId)
        {
            return await _miembroService.DeleteMiembro(miembroId); 
        }
    }
}
