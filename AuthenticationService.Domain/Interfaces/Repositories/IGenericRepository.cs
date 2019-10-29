namespace AuthenticationService.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(object id);
        void Add(T obj);
        void Delete(T obj);
        void Update(T obj);
    }
}
