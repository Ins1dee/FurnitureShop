namespace Application.Abstractions;

public interface IReportService
{
    public Task<string> GenerateHtmlFromRazorPage<T>(string pageName, T model);
}