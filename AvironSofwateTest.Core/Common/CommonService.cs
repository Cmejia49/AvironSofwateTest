using AvironSofwateTest.DataAccess.Context;
using AvironSofwateTest.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvironSofwateTest.Core.Common
{
    public abstract class CommonService : IDisposable
    {
        protected IUnitOfWork<ApplicationDbContext> UnitOfWork { get; private set; }

        protected CommonService(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }
        }
    }
}
