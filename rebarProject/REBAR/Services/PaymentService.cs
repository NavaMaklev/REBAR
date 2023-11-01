using MongoDB.Driver;
using REBAR.Models;

namespace REBAR.Services
{
    public class PaymentService
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentService(IMongoDatabase database)
        {
            _payments = database.GetCollection<Payment>("payments");
        }
        public bool ProcessPayment(Order order)
        {
            // כאן תהיה הלוגיקה של התשלום. בדמו הנוכחי, נחזיר תמיד true.
            return true;
        }
        public void Create(Payment payment)
        {
            _payments.InsertOne(payment);
        }
    }

}
