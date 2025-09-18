using membresias.be.Services;
using Microsoft.AspNetCore.Mvc;

namespace membresias.be.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CargoController : ControllerBase
    {
        private readonly CargoService _cargoService; 

        public CargoController(CargoService cargoService)
        {
            _cargoService = cargoService; 
        }

        [HttpPost("CreateCargos")]
        public async Task<bool> CreateCargosAsync()
        {
            return await _cargoService.CreateCargos(); 
        }
    }
}
