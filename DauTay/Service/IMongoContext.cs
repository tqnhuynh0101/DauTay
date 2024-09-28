using MongoDB.Driver;

namespace DauTay.Service;

public interface IMongoContext : IDisposable
{
	/// <summary>
	/// Lưu trữ các hàm làm việc với database ví dụ: Insert, InsertMany, UpdateOne, UpdateMany,...
	/// </summary>
	/// <param name="func"></param>
	void AddCommand(Func<Task> func);
	Task<int> SaveChanges();
	Task<int> SaveChangesTransaction(IClientSessionHandle session);

	IMongoCollection<T> GetCollection<T>(string name);
}
