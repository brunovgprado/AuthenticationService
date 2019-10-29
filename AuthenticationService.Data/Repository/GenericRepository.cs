using AuthenticationService.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public void Add(T obj)
        {
            throw new NotImplementedException();
        }

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(T obj)
        {
            throw new NotImplementedException();
        }
        public void Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
