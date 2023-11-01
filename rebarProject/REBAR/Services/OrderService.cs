using MongoDB.Driver;
using REBAR.Models;
using REBAR.Configuration;
using System.Collections.Generic;

namespace REBAR.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly ShakeService _shakeService;
        private readonly PriceEntryService _priceEntryService;
        public OrderService(IMongoDatabase database, ShakeService shakeService, PriceEntryService priceEntryService)
        {
            _orders = database.GetCollection<Order>("Orders");
            _shakeService = shakeService;
            _priceEntryService = priceEntryService;
        }
        public List<Order> GetAll() => _orders.Find(order => true).ToList();
        public Order GetById(string id) => _orders.Find<Order>(order => order.Id == Guid.Parse(id)).FirstOrDefault();
        public Order Create(Order order)
        {
            _orders.InsertOne(order);
            return order;
        }
        // ... Additional CRUD operations ...
        public Order CreateOrderFromInput(UserInput orderInput)
        {
            List<string> ids = ShakesIdInOrder(orderInput.Shakes);
            decimal totalPrice = CalculateTotalPriceFromInput(orderInput);
            var finalPrice = orderInput.Discount.CalculatePrice(totalPrice);
            finalPrice = orderInput.Sale.CalculatePrice(finalPrice);
            Order newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = orderInput.CustomerName,
                Date = DateTime.Now,
                ShakesId = ids,
                CompletionTime = DateTime.Now,
                TotalPrice = totalPrice,
                FinalPrice = finalPrice,
                Discounts = orderInput.Discount,
                Sale = orderInput.Sale,
            };

            return newOrder;
        }
        public List<string> ShakesIdInOrder(List<OrderedShake> OrderedShakes)
        {
            List < string > newL= new List<string>();
            foreach (var shakeSelection in OrderedShakes)
            {
                Shake currentShake = _shakeService.GetByName(shakeSelection.Shake);
                if(currentShake != null&& shakeSelection.Quantity>0)
                    newL.Add(currentShake.Id.ToString());
            }
            return newL;
        }
        private decimal CalculateTotalPriceFromInput(UserInput orderInput)
        {
            decimal totalPrice = 0;
            foreach (var shakeSelection in orderInput.Shakes)
            {
                Shake currentShake = _shakeService.GetByName(shakeSelection.Shake);
                if (currentShake != null)
                {
                    PriceEntry price = _priceEntryService.GetBySizeAndIsSpecial(shakeSelection.Size, currentShake.IsSpecial);
                    if(price!=null)
                    {
                        totalPrice += shakeSelection.Quantity * price.Price;
                    }
                }
               
            }
            return totalPrice;
        }

        //Order validations
        public bool IsValidCustomerName(string customerName)
        {
            return !string.IsNullOrEmpty(customerName);
        }
        public bool IsValidShakes(List<OrderedShake> shakes)
        {
            if (shakes == null || !shakes.Any() || shakes.Count > 10 || shakes.Sum(shake => shake.Quantity) > 10 || shakes.Sum(shake => shake.Quantity) == 0)
            {
                return false;
            }
            return true;
        }
        public bool IsValidDiscountAndSale(Discount discount, Sale sale)
        {
            if (discount.AmountToReduced < 0 || sale.PercentagesReduction < 0 || sale.PercentagesReduction >= 100)
            {
                return false;
            }
            return true;
        }

    }
}
