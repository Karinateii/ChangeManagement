using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Change.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRequestRepository Request { get; }
        void Save();
        Task<int> SaveAsync();
    }
}
