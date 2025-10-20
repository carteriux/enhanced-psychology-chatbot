using System;
using System.Threading.Tasks;

using CPC.Infraestructure.Crosscutting.DataObjects.Contracts;

using Microsoft.EntityFrameworkCore;

namespace CPC.Infraestructure.Crosscutting.DataObjects.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            this._context = context;
        }

        public async Task<int> Commit()
        {
            return await this._context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
