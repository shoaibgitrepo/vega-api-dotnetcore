namespace vega_api_dotnetcore.Extensions
{
    public interface IQueryObject
    {
        string SortBy { get; set; }
        bool IsSortAscending { get; set; }
        int? MakeId { get; set; }
        int? ModelId { get; set; }
        int Page { get; set; }
        byte PageSize { get; set; }
    }
}