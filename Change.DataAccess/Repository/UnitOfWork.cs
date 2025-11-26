using Change.DataAccess.Data;
using Change.DataAccess.Repository.IRepository;
using Change.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IRequestRepository Request { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Request = new RequestRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
