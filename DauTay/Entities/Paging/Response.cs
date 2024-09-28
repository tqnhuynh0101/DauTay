using System.Net;

namespace DauTay.Entities.Paging;

public class Response<T>
{
	public T Data { get; set; }
	public int Status { get; set; }
	public bool Succeeded { get; set; }
	public string Message { get; set; }
	public string[] Errors { get; set; } // ghi nhận array lỗi
	//public VersionDTO Version { get; set; } = new VersionDTO();

	public Response()
	{
		Succeeded = false;
		Message = string.Empty;
		Status = (int)HttpStatusCode.Unused;
	}

	public Response(T data)
	{
		Data = data;
		Succeeded = true;
		Message = "Thành công";
		Status = (int)HttpStatusCode.OK;
	}

	public Response(T data, bool success, string message, int status, string[] err)
	{
		Data = data;
		Succeeded = success;
		Message = message;
		Status = status;
		Errors = err;
	}

	public Response(T data, string message)
	{
		Data = data;
		Succeeded = true;
		Message = message;
		Status = (int)HttpStatusCode.OK;
	}
}
