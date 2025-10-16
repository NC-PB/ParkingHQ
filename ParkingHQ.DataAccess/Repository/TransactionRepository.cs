using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    internal class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private ApplicationDbContext _db;

        public TransactionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Transaction obj)
        {
            _db.Transactions.Update(obj);
        }

    }
}
