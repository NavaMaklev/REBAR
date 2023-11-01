using REBAR.Models;

namespace REBAR.Services
{
    public class MenuService
    {
        private readonly ShakeService _shakeService;
        private readonly PriceTableService _priceTableService;

        public MenuService(ShakeService shakeService, PriceTableService priceTableService)
        {
            _shakeService = shakeService;
            _priceTableService = priceTableService;
        }

        public Menu CreateMenu()
        {
            var shakes = _shakeService.GetAll();
            var priceTable = _priceTableService.GetPriceTable();

            return new Menu
            {
                Shakes = shakes,
                PriceTable = priceTable
            };
        }
    }

}
