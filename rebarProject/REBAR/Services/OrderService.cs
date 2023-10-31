using MongoDB.Driver;
using REBAR.Models;
using REBAR.Configuration;

namespace REBAR.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderService(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("Orders");
        }

        public List<Order> GetAll() => _orders.Find(order => true).ToList();

        public Order GetById(string id) => _orders.Find<Order>(order => order.Id == Guid.Parse(id)).FirstOrDefault();

        public Order Create(Order order)
        {
            _orders.InsertOne(order);
            return order;
        }

        // ... Additional CRUD operations ...
    }
}
