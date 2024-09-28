using DauTay.Interfaces;
using DauTay.Interfaces.Category;
using DauTay.Repositories.Category;
using DauTay.Service;
using MongoDB.Driver;

namespace DauTay.Repositories;
public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly IMongoContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly Lazy<IProductTypeRepository> _productType;
    public IProductTypeRepository ProductType => _productType.Value;

    public RepositoryWrapper(IMongoContext context, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _productType = new Lazy<IProductTypeRepository>(() => new ProductTypeRepository(_context, _contextAccessor));

    }

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
        int changeAmount = await _context.SaveChangesTransaction(session);
        Dispose();
        return changeAmount > 0;
    }
}
