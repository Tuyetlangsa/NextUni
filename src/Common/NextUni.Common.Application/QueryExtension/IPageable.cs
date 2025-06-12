namespace NextUni.Common.Application.QueryExtension;

public interface IPageable
{
    public int PageNumber { get; }
    public int PageSize { get; }
}