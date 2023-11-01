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
        private readonly BranchAccountService _branchAccountService;
        public OrderController(OrderService orderService, PaymentService paymentService, ShakeService shakeService, PriceEntryService priceEntryService, BranchAccountService branchAccountService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _shakeService = shakeService;
            _priceEntryService = priceEntryService;
            _branchAccountService = branchAccountService;
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
            if (!_branchAccountService.IsBranchAccountExists(orderInput.BranchID))
            {
                return BadRequest("You are trying to add an order to an account that does not exist.");
            }
            if (!_orderService.IsValidCustomerName(orderInput.CustomerName))
            {
                return BadRequest("The customer name cannot be empty.");
            }
            if (!_orderService.IsValidShakes(orderInput.Shakes))
            {
                return BadRequest("At least one shake must be selected.You cannot select more than 10 shakes");
            }
            if (!_orderService.IsValidDiscountAndSale(orderInput.Discount,orderInput.Sale))
            {
                return BadRequest("The discount/sale must be a positive number,sale must be a positive number between 0 to 100");
            }           
            Order newOrder =_orderService. CreateOrderFromInput(orderInput);
            if (!_branchAccountService.AddOrderToBranchAccount(newOrder,orderInput.BranchID))
                return BadRequest("You are trying to add an order to a branch that does not exist");
            //Payment for the order
            Pay(newOrder);
            // Add order
            _orderService.Create(newOrder);
            return Ok("The order was successfully created.");
        }
        [HttpPost("pay")]
        public ActionResult Pay(Order order)
        {

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
