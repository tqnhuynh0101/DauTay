using MongoDB.Driver;
using DauTay.Interfaces.Category;
namespace DauTay.Interfaces;

public interface IRepositoryWrapper : IDisposable
{
	#region Category
	IProductTypeRepository ProductType { get;}
	
	#endregion

	Task<bool> CommitAsync();
	Task<bool> CommitAsyncTransaction(IClientSessionHandle session);
}
