using DauTay.Entities.Base;
using DauTay.Interfaces;
using DauTay.Service;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;

namespace DauTay.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : IEntityBase
{
    protected readonly IMongoContext _context;
    protected readonly IHttpContextAccessor _accessor;

    protected IMongoCollection<T> _mongoCollection;
    private readonly FilterDefinition<T> _filterBase;
    private FilterDefinition<T> _filterFinal;

    /// <summary>
    /// Username đăng nhập 
    /// </summary>
    protected string CurrentUser => _accessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;


    public RepositoryBase(IMongoContext context, IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
        _mongoCollection = _context.GetCollection<T>(typeof(T).Name);
        _filterBase = Builders<T>.Filter.Eq("IsActive", true);
        _filterFinal = FilterDefinition<T>.Empty;

    }

    public async Task<List<BsonDocument>> Aggregate(List<BsonDocument> pipelines, string viewName = null)
    {
        try { if (!string.IsNullOrEmpty(viewName)) _mongoCollection = _context.GetCollection<T>(viewName); }
        catch (Exception) { throw new Exception(viewName + " không tồn tại"); }
        return await _mongoCollection.Aggregate<BsonDocument>(pipelines).ToListAsync().ConfigureAwait(false);
    }

    public bool CheckExist(FilterDefinition<T> filter)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;
        return _mongoCollection.Find(_filterFinal).Any();
    }

    public Task<bool> CheckExistAsync(FilterDefinition<T> filter)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;

        return Task.Run(() => _mongoCollection.Find(_filterFinal).AnyAsync());
    }

    public void Delete(string id, IClientSessionHandle session = null)
    {
        //check null
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException($"{typeof(T).Name}: Base.Delete - id is null");
        }

        FilterDefinition<T> filter = new BsonDocument("_id", new ObjectId(id));
        UpdateDefinition<T> update = new BsonDocument("$set", new BsonDocument {
                { "IsActive", false }, { "UpdatedOn", DateTime.Now }, { "UpdatedBy", CurrentUser }
            });

        if (session == null)
            _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(filter, update));
        else
            _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(session, filter, update));
    }

    public void DeleteForeverAsync(FilterDefinition<T> filter, IClientSessionHandle clientSession = null, bool skipFilterBase = false)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;
        if (skipFilterBase == true)
        {
            _filterFinal = filter;
        }

        if (clientSession == null)
            _context.AddCommand(async () => await _mongoCollection.DeleteOneAsync(filter).ConfigureAwait(false));
        else
            _context.AddCommand(async () => await _mongoCollection.DeleteOneAsync(clientSession, filter).ConfigureAwait(false));
    }

    public void DeleteManyForeverAsync(FilterDefinition<T> filter, IClientSessionHandle clientSession = null, bool skipFilterBase = false)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;
        if (skipFilterBase == true)
        {
            _filterFinal = filter;
        }
        if (clientSession == null)
            _context.AddCommand(async () => await _mongoCollection.DeleteManyAsync(filter).ConfigureAwait(false));
        else
            _context.AddCommand(async () => await _mongoCollection.DeleteManyAsync(clientSession, filter).ConfigureAwait(false));
    }

    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }

    public T FindFirstOne(FilterDefinition<T> filter)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;
        return _mongoCollection.Find(_filterFinal).FirstOrDefault();
    }

    public Task<T> FindFirstOneAsync(FilterDefinition<T> filter)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;

        return Task.Run(() => _mongoCollection.Find(_filterFinal).FirstOrDefaultAsync());
    }

    public T FindLastOne(FilterDefinition<T> filter)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;

        SortDefinition<T> sort = Builders<T>.Sort.Descending("Id");
        return _mongoCollection.Find(_filterFinal).Sort(sort).FirstOrDefault();
    }

    public Task<T> FindLastOneAsync(FilterDefinition<T> filter)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;
        SortDefinition<T> sort = Builders<T>.Sort.Descending("Id");
        return Task.Run(() => _mongoCollection.Find(_filterFinal).Sort(sort).FirstOrDefaultAsync());
    }

    public T FindOne(FilterDefinition<T> filter, SortDefinition<T> sort = null)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;
        if (sort is null)
            return _mongoCollection.Find(_filterFinal).FirstOrDefault();
        else
            return _mongoCollection.Find(_filterFinal).Sort(sort).FirstOrDefault();
    }

    public Task<T> FindOneAsync(FilterDefinition<T> filter, SortDefinition<T> sort = null)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;
        if (sort is null)
            return Task.Run(() => _mongoCollection.Find(_filterFinal).FirstOrDefaultAsync());
        else
            return Task.Run(() => _mongoCollection.Find(_filterFinal).Sort(sort).FirstOrDefaultAsync());
    }

    public List<T> GetAll(FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
    {
        _filterFinal = _filterBase;
        if (filter != null)
        {
            _filterFinal &= filter;
        }
        return sort != null ? _mongoCollection.Find(_filterFinal).Sort(sort).ToList() : _mongoCollection.Find(_filterFinal).ToList();
    }

    public Task<List<T>> GetAllAsync(FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
    {
        _filterFinal = _filterBase;

        if (filter != null)
        {
            _filterFinal &= filter;
        }
        return sort != null
            ? Task.Run(() => _mongoCollection.Find(_filterFinal).Sort(sort).ToListAsync())
            : Task.Run(() => _mongoCollection.Find(_filterFinal).ToListAsync());
    }

    public T GetById(string id, FilterDefinition<T> filter = null)
    {
        _filterFinal = _filterBase;

        _filterFinal &= Builders<T>.Filter.Eq("_id", new ObjectId(id));

        if (filter != null)
        {
            _filterFinal &= filter;
        }

        return _mongoCollection.Find(_filterFinal).FirstOrDefault();
    }

    public Task<T> GetByIdAsync(string id, FilterDefinition<T> filter = null)
    {
        _filterFinal = _filterBase;

        _filterFinal &= Builders<T>.Filter.Eq("_id", new ObjectId(id));

        if (filter != null)
        {
            _filterFinal &= filter;
        }
        return Task.Run(() => _mongoCollection.Find(_filterFinal).FirstOrDefaultAsync());
    }

    public Task<List<T>> GetListToViewAsync(string viewName, FilterDefinition<T> filter = null, ProjectionDefinition<T, T> projection = null, SortDefinition<T> sort = null)
    {
        throw new NotImplementedException();
    }

    public void Insert(T t, IClientSessionHandle session = null)
    {

        t.Id = null;

        if (session == null)
            _context.AddCommand(async () => await _mongoCollection.InsertOneAsync(t));
        else
            _context.AddCommand(async () => await _mongoCollection.InsertOneAsync(session, t));
    }

    public void InsertMany(IList<T> list, IClientSessionHandle session = null)
    {
        InsertManyOptions options = new() { IsOrdered = true };
        foreach (T t in list)
        {

            t.Id = null;
        }
        if (session == null)
            _context.AddCommand(async () => await _mongoCollection.InsertManyAsync(list, options));
        else
            _context.AddCommand(async () => await _mongoCollection.InsertManyAsync(session, list, options));
    }

    public async Task InsertManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null)
    {
        InsertManyOptions options = new() { IsOrdered = true };
        foreach (T t in list)
        {

            t.Id = null;
        }
        if (session == null)
            await _mongoCollection.InsertManyAsync(list, options);
        else
            await _mongoCollection.InsertManyAsync(session, list, options);
    }

    public async Task<string> InsertWithoutCommandTask(T t, IClientSessionHandle session = null)
    {

        t.Id = null;

        if (session == null)
            await _mongoCollection.InsertOneAsync(t);
        else
            await _mongoCollection.InsertOneAsync(session, t);

        return t.Id;
    }

    public List<T> SearchMatchArray(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
    {
        _filterFinal = _filterBase;
        //reset
        _filterFinal &= Builders<T>.Filter.In(docPropertyName, lstValue.Distinct());

        if (filter != null)
        {
            _filterFinal &= filter;
        }

        return sort != null ? _mongoCollection.Find(_filterFinal).Sort(sort).ToList() : _mongoCollection.Find(_filterFinal).ToList();
    }

    public Task<List<T>> SearchMatchArrayAsync(string docPropertyName, List<string> lstValue, FilterDefinition<T> filter = null, SortDefinition<T> sort = null)
    {
        _filterFinal = _filterBase;
        //reset
        _filterFinal &= Builders<T>.Filter.In(docPropertyName, lstValue.Distinct());

        if (filter != null)
        {
            _filterFinal &= filter;
        }

        return sort != null
            ? Task.Run(() => _mongoCollection.Find(_filterFinal).Sort(sort).ToListAsync())
            : Task.Run(() => _mongoCollection.Find(_filterFinal).ToListAsync());
    }

    public void Update(string id, T t, string[] arrExceptField = null, IClientSessionHandle session = null, string[] arrActiveField = null)
    {
        UpdateDefinitionBuilder<T> builder = Builders<T>.Update;
        UpdateDefinition<T> update = null;
        // Set value cho các trường mặc định
        //t.Id = null;
        t.IsActive = true;

        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            CustomAttributeData ignore = prop.CustomAttributes.ToList().Find(x => x.AttributeType.Name is "BsonIgnoreAttribute" or "BsonIgnoreIfNullAttribute");
            if (ignore != null)
            {
                continue;
            }
            // Mongo không cho phép thay đổi Mongo IDs
            if (prop.PropertyType == typeof(ObjectId))
            {
                continue;
            }
            // Những trường nào thuộc arrExceptField thì không update
            if (arrExceptField != null && arrExceptField.Contains(prop.Name))
            {
                continue;
            }
            // Nếu không có giá trị nào được gán thì giữ nguyên cái đang có dưới database
            if (prop.GetValue(t) == null && prop.PropertyType != typeof(DateTime?)) // set datetime? = null
            {
                if (arrActiveField != null && arrActiveField.Contains(prop.Name))
                {
                    // vẫn update cả khi null
                }
                else
                    continue;
            }
            // Nếu là thuộc tính datetime thì C# tự gán ngày mặc định chứ không phải null, phải kiểm tra và bỏ qua nó
            if (prop.PropertyType == typeof(DateTime))
            {
                if (Convert.ToDateTime(prop.GetValue(t)) == DateTime.MinValue)
                {
                    continue;
                }
            }
            // Gán lần đầu tiên, cũng chính là gán Object Id
            update = update == null ? builder.Set(prop.Name, prop.GetValue(t)) : update.Set(prop.Name, prop.GetValue(t));
        }

        BsonDocument filter = new("_id", new ObjectId(id));

        UpdateOptions option = new()
        {
            IsUpsert = false
        };

        if (session == null)
            _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(filter, update, option).ConfigureAwait(false));
        else
            _context.AddCommand(async () => await _mongoCollection.UpdateOneAsync(session, filter, update, option).ConfigureAwait(false));
    }

    public void UpdateCustomizeField(FilterDefinition<T> filter, BsonDocument bsUpdate, IClientSessionHandle session = null)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;

        BsonDocument bs = new(bsUpdate)
            {
                { "UpdatedOn", DateTime.Now },
                { "UpdatedBy", CurrentUser is null ? string.Empty : CurrentUser }
            };
        UpdateDefinition<T> update = new BsonDocument("$set", bs);
        if (session == null)
            _context.AddCommand(async () => await _mongoCollection.UpdateManyAsync(filter, update).ConfigureAwait(false));
        else
            _context.AddCommand(async () => await _mongoCollection.UpdateManyAsync(session, filter, update).ConfigureAwait(false));
    }

    public void UpdateMany(IList<T> list, IClientSessionHandle session = null)
    {
        List<WriteModel<T>> updates = [];
        FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        foreach (T document in list)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (prop.Name == "Id")
                {
                    if (prop.GetValue(document) != null)
                    {
                        FilterDefinition<T> filter = filterBuilder.Eq(prop.Name, prop.GetValue(document));

                        updates.Add(new ReplaceOneModel<T>(filter, document));
                        break;
                    }
                }
            }
        }
        if (session == null)
            _context.AddCommand(async () => await _mongoCollection.BulkWriteAsync(updates));
        else
            _context.AddCommand(async () => await _mongoCollection.BulkWriteAsync(session, updates));
    }

    public async Task<bool> UpdateManyWithoutCommandTask(IList<T> list, IClientSessionHandle session = null)
    {
        List<WriteModel<T>> updates = [];
        FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        foreach (T document in list)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (prop.Name == "Id")
                {
                    if (prop.GetValue(document) != null)
                    {
                        FilterDefinition<T> filter = filterBuilder.Eq(prop.Name, prop.GetValue(document));

                        updates.Add(new ReplaceOneModel<T>(filter, document));

                        break;
                    }
                }
            }
        }
        BulkWriteResult result = session == null
            ? await _mongoCollection.BulkWriteAsync(updates).ConfigureAwait(false)
            : (BulkWriteResult)await _mongoCollection.BulkWriteAsync(session, updates).ConfigureAwait(false);
        return result.IsModifiedCountAvailable;
    }

    public async Task<long> CountDocumentAsync(FilterDefinition<T> filter)
    {
        _filterFinal = _filterBase;
        _filterFinal &= filter;

        return await _mongoCollection.CountDocumentsAsync(filter);
    }
}
