namespace NextUni.Common.Application.QueryExtension;

public interface ISortable
{
    public string? SortColumn { get; }
    public SortOrder SortOrder { get; }
}