using Change.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change.DataAccess.Repository.IRepository
{
    public interface IRequestRepository : IRepository<Request>
    {
        void update(Request obj);
    }
}
