using Microsoft.AspNetCore.Mvc;
using REBAR.Models;
using REBAR.Services;
using System.Xml.Linq;

namespace REBAR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly PaymentService _paymentService;
        private readonly ShakeService _shakeService;
        private readonly PriceEntryService _priceEntryService;
        public OrderController(OrderService orderService, PaymentService paymentService, ShakeService shakeService, PriceEntryService priceEntryService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _shakeService = shakeService;
            _priceEntryService = priceEntryService;
        }

        [HttpGet]
        public ActionResult<List<Order>> GetAll() => _orderService.GetAll();

        [HttpGet("{id}", Name = "GetOrder")]
        public ActionResult<Order> GetById(string id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public IActionResult CreateOrder(UserInput orderInput)
        {
            DateTime created = DateTime.Now;
            if (string.IsNullOrEmpty(orderInput.CustomerName))
            {
                return BadRequest("The customer name cannot be empty.");
            }

            if (orderInput.Shakes == null || !orderInput.Shakes.Any())
            {
                return BadRequest("At least one shake must be selected.");
            }

            if (orderInput.Shakes.Count > 10|| orderInput.Shakes.Sum(shake => shake.Quantity)>10)
            {
                return BadRequest("You cannot select more than 10 shakes.");
            }
            if ( orderInput.Shakes.Sum(shake => shake.Quantity) ==0)
            {
                return BadRequest("You dont select  shakes.");
            }
            if (orderInput.Discount.AmountToReduced<0||orderInput.Sale.PercentagesReduction<0)
            {
                return BadRequest("The discount/sale must be a positive number");
            }
            if (orderInput.Sale.PercentagesReduction >= 100)
            {
                return BadRequest("The sale must be a positive number between 0 to 100");
            }
            List<Guid> shakesID = new List<Guid>();
            decimal totalPrice=0;
            foreach (var shakeSelection in orderInput.Shakes)
            {
                Shake currentShake = _shakeService.GetByName(shakeSelection.Shake);
                if (currentShake == null)
                {
                    return BadRequest($"Shake {shakeSelection.Shake} not found.");
                }
                PriceEntry price = _priceEntryService.GetBySizeAndIsSpecial(shakeSelection.Size, currentShake.IsSpecial);
                if(shakeSelection.Quantity==0)
                {
                    return BadRequest("You must select Quantity.");
                }
                shakesID.Add(currentShake.Id);
                totalPrice += shakeSelection.Quantity * price.Price;
            }
            var finalPrice = orderInput.Discount.CalculatePrice(totalPrice);
            finalPrice=orderInput.Sale.CalculatePrice(finalPrice);
            Order newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = orderInput.CustomerName,
                Date = created,
                ShakesId = shakesID,
                CompletionTime= DateTime.Now,
                TotalPrice= totalPrice,
                FinalPrice= finalPrice,
                Discounts=orderInput.Discount,
                Sale=orderInput.Sale,
            };

            // Here you would save the 'newOrder' object to MongoDB...
            Pay(newOrder);
             _orderService.Create(newOrder);
            //return CreatedAtRoute("GetOrder", new { id = newOrder.Id.ToString() }, newOrder);
            return Ok("The order was successfully created.");
        }
        [HttpPost("pay")]
        public ActionResult Pay(Order order)
        {
            // בדיקה של הנתונים...

            bool paymentSucceeded = _paymentService.ProcessPayment(order);

            if (!paymentSucceeded)
            {
                return BadRequest("Payment failed.");
            }

            var payment = new Payment
            {
                OrderId = order.Id.ToString(),
                PaymentDate = DateTime.Now,
                PaymentAmount=order.TotalPrice
            };

            _paymentService.Create(payment);

            return Ok();
        }
        
    }
}
