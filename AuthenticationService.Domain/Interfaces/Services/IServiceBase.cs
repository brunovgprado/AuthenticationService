using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Interfaces.Services
{
    public interface IServiceBase<T> where T : class
    {
        T Add(T entity);
        T Select(object id);
        void Update(T entity);
    }
}
