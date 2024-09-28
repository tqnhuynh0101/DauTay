using DauTay.Entities.Base;
using DauTay.Interfaces;
using DauTay.Service;
using MongoDB.Driver;
using System.Collections;

namespace DauTay.Repositories;

public class UnitOfWork(IMongoContext context, IHttpContextAccessor contextAccessor) : IUnitOfWork
{
    private Hashtable _repositories;
    private readonly IMongoContext _context = context;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;


    public async Task<bool> CommitAsync()
    {
        int changeAmount = await _context.SaveChanges();
        Dispose();
        return changeAmount > 0;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<bool> CommitAsyncTransaction(IClientSessionHandle session)
    {
        //session.CommitTransaction();
        int changeAmount = await _context.SaveChangesTransaction(session);
        Dispose();
        return changeAmount > 0;
    }
    //Hell Kaiser
    //Hàm này trả về một repository cụ thể dựa vào kiểu TEntity
    public IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : EntityBase
    {
        _repositories ??= [];

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, _contextAccessor);

            _repositories.Add(type, repositoryInstance);
        }

        return (IRepositoryBase<TEntity>)_repositories[type];
    }
}