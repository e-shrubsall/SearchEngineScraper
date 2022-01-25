using WebApp.Scrapers;

namespace WebApp;

public class QueryRequest
{
    public string QueryText { get; set; }
    public string TargetUrl { get; set; }
    public SearchEngine Engine { get; set; }
}