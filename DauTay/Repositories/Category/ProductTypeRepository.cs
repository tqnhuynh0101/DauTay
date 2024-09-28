using DauTay.Entities.Category;
using DauTay.Interfaces.Category;
using DauTay.Service;

namespace DauTay.Repositories.Category
{
    public class ProductTypeRepository(IMongoContext context, IHttpContextAccessor accessor) : RepositoryBase<ProductType>(context, accessor), IProductTypeRepository
    {
    }
}
