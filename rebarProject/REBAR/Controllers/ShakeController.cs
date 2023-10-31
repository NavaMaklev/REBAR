using Microsoft.AspNetCore.Mvc;
using REBAR.Models;
using REBAR.Services;
using System.Xml.Linq;

namespace REBAR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShakeController : ControllerBase
    {
        private readonly ShakeService _shakeService;

        public ShakeController(ShakeService shakeService)
        {
            _shakeService = shakeService;
        }

        [HttpGet]
        public ActionResult<List<Shake>> GetAll() => _shakeService.GetAll();

        [HttpGet("{name}", Name = "GetShake")]
        public ActionResult<Shake> GetByName(string name)
        {
            var shake = _shakeService.GetByName(name);
            if (shake == null)
            {
                return NotFound();
            }
            return shake;
        }

        [HttpPost]
        public ActionResult<Shake> Create(Shake shake)
        {
            shake.Id = Guid.NewGuid();
            var existingShake = _shakeService.GetByName(shake.Name); // Assuming you have a method GetByName in your ShakeService

            if (existingShake != null)
            {
                return BadRequest("A shake with this name already exists. Please choose a different name.");
            }
            _shakeService.Create(shake);
            return CreatedAtRoute("GetShake", new { name = shake.Name.ToString() }, shake);
        }

        
    }

}
