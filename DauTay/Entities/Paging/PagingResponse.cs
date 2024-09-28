namespace DauTay.Entities.Paging;

public class PagingResponse<T> : Response<T>
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public int TotalPages { get; set; }
	public long TotalRecords { get; set; }

	public PagingResponse(T data, int pageNumber, int pageSize)
	{
		PageNumber = pageNumber;
		PageSize = pageSize;
		Data = data;
		Message = null;
		Succeeded = true;
		Status = 200;
	}
}
