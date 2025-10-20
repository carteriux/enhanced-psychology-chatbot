using System;
using System.Threading.Tasks;

namespace CPC.Infraestructure.Crosscutting.DataObjects.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
    }
}
