using Microsoft.AspNetCore.Mvc;
using REBAR.Models;
using REBAR.Services;

namespace REBAR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceTableController : ControllerBase
    {
        private readonly PriceTableService _priceTableService;
        public PriceTableController(PriceTableService priceTableService)
        {
            _priceTableService = priceTableService;
        }
        [HttpGet("pricetable")]
        public ActionResult<PriceTable> GetPriceTable()
        {
            return _priceTableService.GetPriceTable();
        }
    }
}
