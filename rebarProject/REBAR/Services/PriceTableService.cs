using REBAR.Models;

namespace REBAR.Services
{
    public class PriceTableService
    {
        private readonly PriceEntryService _priceEntryService; 

        public PriceTableService(PriceEntryService priceEntryService)
        {
            _priceEntryService = priceEntryService;
        }
        public PriceTable GetPriceTable()
        {
            var allPrices = _priceEntryService.GetAll();
            var regularPrices = allPrices.Where(p => !p.IsSpecial).ToList();
            var specialPrices = allPrices.Where(p => p.IsSpecial).ToList();

            return new PriceTable
            {
                RegularPrices = regularPrices,
                SpecialPrices = specialPrices
            };
        }

    }
}
