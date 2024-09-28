using MongoDB.Driver;

namespace DauTay.Service;

public class MongoContext : IMongoContext
{
	private IMongoDatabase Database { get; set; }
	public MongoClient MongoClient { get; set; }
	private readonly List<Func<Task>> _commands;

	public MongoContext()
	{
		// Tất cả các câu lệnh(thêm, sửa, delete,...) sẽ được lưu vào đây, sẽ được thực thi lúc SaveChanges
		_commands = [];
	}

	public void AddCommand(Func<Task> func)
	{
		_commands.Add(func);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
	}

	public IMongoCollection<T> GetCollection<T>(string name)
	{
		ConfigureMongo();
		return Database.GetCollection<T>(name);
	}

	public async Task<int> SaveChanges()
	{
		IEnumerable<Task> commandTasks = _commands.Select(c => c());
		await Task.WhenAll(commandTasks);
		Dispose();
		return _commands.Count;
	}


	public async Task<int> SaveChangesTransaction(IClientSessionHandle session)
	{
		using (session)
		{
			session.StartTransaction();
			IEnumerable<Task> commandTasks = _commands.Select(c => c());
			await Task.WhenAll(commandTasks);
			session.CommitTransaction();
		}
		Dispose();
		return _commands.Count;
	}

	private void ConfigureMongo()
	{
		MongoClient ??= new MongoClient(MongoSetting.Connection);
		Database = MongoClient.GetDatabase(MongoSetting.DatabaseName);
	}
}
