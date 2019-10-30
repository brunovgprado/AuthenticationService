using AuthenticationService.Domain.Interfaces.Services;
using AuthenticationService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Domain.Services
{
    public class ServiceBase<T> : IServiceBase<T> where T : class
    {
        public T Add(T entity)
        {
            throw new NotImplementedException();
        }

        public T Select(object id)
        {
            throw new NotImplementedException();
        }
    }
}
