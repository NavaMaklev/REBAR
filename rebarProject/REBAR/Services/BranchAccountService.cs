using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using REBAR.Models;

namespace REBAR.Services
{
    public class BranchAccountService
    {
        private readonly IMongoCollection<BranchAccount> _branchAccounts;
        public BranchAccountService(IMongoDatabase database)
        {
            _branchAccounts = database.GetCollection<BranchAccount>("BranchAccounts");
        }
        public List<BranchAccount> Get() =>
        _branchAccounts.Find(branchAccount => true).ToList();

        public BranchAccount Get(Guid branchId) =>
            _branchAccounts.Find<BranchAccount>(branchAccount => branchAccount.BranchId == branchId).FirstOrDefault();
        public int GetOrdersCountForToday(Guid branchId)
        {
            var branch = Get(branchId);
            var today = DateTime.Today;
             return branch.Orders.Count(o => o.Date.Date == today.Date);
        }
        public bool IsBranchAccountExists(Guid branchId)
        {
            return Get(branchId) != null;
        }

        public decimal GetOrdersSumForToday(Guid branchId)
        {
            var branch = Get(branchId);
            var today = DateTime.Today;
            return branch.Orders.Where(o => o.Date.Date == today.Date).Sum(o => o.FinalPrice);
        }
        public BranchAccount Create(BranchAccount branchAccount)
        {
            _branchAccounts.InsertOne(branchAccount);
            return branchAccount;
        }

        public void Update(Guid branchId, BranchAccount branchAccountIn) =>
            _branchAccounts.ReplaceOne(branchAccount => branchAccount.BranchId == branchId, branchAccountIn);

        public void Remove(Guid branchId) =>
            _branchAccounts.DeleteOne(branchAccount => branchAccount.BranchId == branchId);

        public bool AddOrderToBranchAccount(Order order, Guid branchId)
        {
            if(IsBranchAccountExists(branchId))
            {
                var branchAccount = Get(branchId);
                branchAccount.Orders.Add(order);
                branchAccount.TotalAmount += order.FinalPrice;
                Update(branchAccount.BranchId, branchAccount);
                return true;
            }
            return false;
        }

        public DailyReport? CloseAccountForToday(Guid branchId)
        {
            if (!IsBranchAccountExists(branchId))
                return null;
            var orderCount = GetOrdersCountForToday(branchId);
            var orderSum = GetOrdersSumForToday(branchId);

            var report = new DailyReport
            {
                BranchId = branchId,
                Date = DateTime.Today,
                TotalOrders = orderCount,
                TotalAmount = orderSum
            };
            BranchAccount ba=Get(branchId);
            ba.Reports.Add(report);
            Update(ba.BranchId, ba);
            return report;
        }
    }
    public class DailyReport
    {
        public Guid BranchId { get; set;}
        public DateTime Date { get; set;}
        public int TotalOrders { get; set;}
        public decimal TotalAmount { get; set;}
    }
}
