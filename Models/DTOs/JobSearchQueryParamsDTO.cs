namespace Webscrapping.Models.DTOs;

public class JobSearchQueryParamsDTO
{
    public string Keyword { get; private set; } = "";
    public int Page { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;
}
