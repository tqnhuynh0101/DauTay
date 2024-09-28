using DauTay.Entities.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DauTay.Interfaces;

public interface IRepositoryBase<T> : IDisposable where T : IEntityBase
{
    /////////// GET
    Task<List<T>> GetAllAsync(FilterDefinition<T> filter = null, SortDefinition<T> sort = null);
    List<T> GetAll(FilterDefinition<T> filter = null, SortDefinition<T> sort = null);
    Task<T> GetByIdAsync(string id, FilterDefinition<T> filter = null);
    T GetById(string id, FilterDefinition<T> filter = null);

    /////////// FIND WITH CONDITION
    Task<T> FindFirstOneAsync(FilterDefinition<T> filter);
    T FindFirstOne(FilterDefinition<T> filter);
    Task<T> FindLastOneAsync(FilterDefinition<T> filter);
    T FindLastOne(FilterDefinition<T> filter);
    Task<T> FindOneAsync(FilterDefinition<T> filter, SortDefinition<T> sort = null);
    T FindOne(FilterDefinition<T> filter, SortDefinition<T> sort = null);

    //////////// CHECK WITH CONDITION
    Task<bool> CheckExistAsync(FilterDefinition<T> filter);
    bool CheckExist(FilterDefinition<T> filter);

    //////////// INSERT
    void Insert(T t, IClientSessionHandle session = null);
    void InsertMany(IList<T> list, IClientSessionHandle session = null);
    Task<string> InsertWithoutCommandTask(T t, IClientSessionHandle session = null);
    Task InsertManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null);

    //////////// UPDATE
    void Update(string id, T t, string[] arrExceptField = null, IClientSessionHandle session = null, string[] arrActiveField = null);
    void UpdateMany(IList<T> list, IClientSessionHandle session = null);
    void UpdateCustomizeField(FilterDefinition<T> filter, BsonDocument bsUpdate, IClientSessionHandle session = null);
    Task<bool> UpdateManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null);

    //////////// DELETE
    void Delete(string id, IClientSessionHandle session = null);
    void DeleteManyForeverAsync(FilterDefinition<T> filter, IClientSessionHandle clientSession = null, bool skipFilterBase = false);
    void DeleteForeverAsync(FilterDefinition<T> filter, IClientSessionHandle clientSession = null, bool skipFilterBase = false);

    //////////// SEARCH
    Task<List<T>> SearchMatchArrayAsync(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null);
    List<T> SearchMatchArray(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null);

    //////////// SUPPORT
    Task<List<BsonDocument>> Aggregate(List<BsonDocument> pipelines, string viewName = null);
    Task<List<T>> GetListToViewAsync(string viewName, FilterDefinition<T> filter = null, ProjectionDefinition<T, T> projection = null, SortDefinition<T> sort = null);

    Task<long> CountDocumentAsync(FilterDefinition<T> filter);
}
