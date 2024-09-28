using DauTay.Entities.Base;
using MongoDB.Driver;

namespace DauTay.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : EntityBase;
        Task<bool> CommitAsync();
        Task<bool> CommitAsyncTransaction(IClientSessionHandle session);
    }
}