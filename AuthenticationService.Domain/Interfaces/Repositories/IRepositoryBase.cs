namespace AuthenticationService.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        T GetById(object id);
        T Add(T obj);
        void Delete(T obj);
        void Update(T obj);
    }
}
