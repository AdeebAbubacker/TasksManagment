using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment.Application.Utilities
{
    public interface IRequestHandler<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);


    }

    public interface IRequestHandler<TRequest>
    {
        Task Handle(TRequest request);


    }
}
