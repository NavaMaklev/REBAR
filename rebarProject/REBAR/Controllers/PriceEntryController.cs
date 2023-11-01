using Microsoft.AspNetCore.Mvc;
using REBAR.Models;
using REBAR.Services;

namespace REBAR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceEntryController : ControllerBase
    {
        private readonly PriceEntryService _priceEntryService;
        public PriceEntryController(PriceEntryService priceEntryService)
        {
            _priceEntryService = priceEntryService;
        }
        [HttpGet]
        public ActionResult<List<PriceEntry>> GetAll() => _priceEntryService.GetAll();
        [HttpGet("{size}", Name = "GetBySize")]
        public ActionResult<PriceEntry> GetBySize(char size)
        {
            var priceEntry = _priceEntryService.GetBySize(size);
            if (priceEntry == null)
            {
                return NotFound();
            }
            return Ok(priceEntry);
        }
        [HttpGet("{size&isSpecial}", Name = "GetBySize&isSpecial")]
        public ActionResult<PriceEntry> GetBySizeAndIsSpecial(char size,bool isSpecial)
        {
            var priceEntry = _priceEntryService.GetBySizeAndIsSpecial(size,isSpecial);
            if (priceEntry == null)
            {
                return NotFound();
            }
            return Ok(priceEntry);
        }
        [HttpPost]
        public ActionResult<PriceEntry> Create(PriceEntry priceEntry)
        {
            priceEntry.Id = Guid.NewGuid();
            var existingPriceEntry = _priceEntryService.GetBySizeAndIsSpecial(priceEntry.Size, priceEntry.IsSpecial);
            if (existingPriceEntry != null)
            {
                return BadRequest("A price for this size in this branch already exists. Please update the price using the appropriate function.");
            }
            _priceEntryService.Create(priceEntry);
            return CreatedAtRoute(nameof(GetBySize), new { size = priceEntry.Size }, priceEntry);
        }
        [HttpPut("{size}")]
        public IActionResult Update(char size, PriceEntry priceEntry)
        {
            var existingPriceEntry = _priceEntryService.GetBySize(size);
            if (existingPriceEntry == null)
            {
                return NotFound();
            }
            _priceEntryService.Update(size, priceEntry);
            return NoContent();
        }
        [HttpDelete("{size}")]
        public IActionResult Delete(char size)
        {
            var priceEntry = _priceEntryService.GetBySize(size);
            if (priceEntry == null)
            {
                return NotFound();
            }
            _priceEntryService.Remove(size);
            return NoContent();
        }
    }
}
