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
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        private ApplicationDbContext _db;

        public RequestRepository (ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void update(Request obj)
        {
            _db.Requests.Update(obj);
        }

    }
}
