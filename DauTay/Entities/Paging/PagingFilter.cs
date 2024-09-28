namespace DauTay.Entities.Paging;

public class PagingFilter : PagingFilterByDate
{
	public int PageNumber { get; set; }

	/// <summary>
	/// Items per page
	/// </summary>
	public int PageSize { get; set; }

	/// <summary>
	/// Text Search, find all columns
	/// </summary>
	public string Text { get; set; }

	/// <summary>
	/// Column name want to sort
	/// </summary>
	public string SortColumn { get; set; }
	/// <summary>
	/// Sort by ASC or DESC
	/// </summary>
	public string SortColumnDirection { get; set; }

	/// <summary>
	/// Filter theo column
	/// </summary>
	public string Columns { get; set; }

	public PagingFilter()
	{
		PageNumber = 1; // first page
		PageSize = 20;  // default items per page
		SortColumn = "id"; // Default by ObjectId
		SortColumnDirection = "desc";  // default descending desc
	}
	public PagingFilter(int pageNumber, int pageSize, string sortColumn, string direction)
	{
		PageNumber = pageNumber < 1 ? 1 : pageNumber;
		PageSize = (pageSize <= 0) ? 20 : pageSize;
		SortColumn = string.IsNullOrEmpty(sortColumn) ? "id" : sortColumn;
		SortColumnDirection = string.IsNullOrEmpty(direction) ? "desc" : direction;
	}
}
