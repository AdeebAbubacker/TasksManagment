using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TaskManagment.Application.Contracts.Persistance
{
    public interface IUnitOfWork
    {
        Task Commit();

        Task Rollback();
    }
}
