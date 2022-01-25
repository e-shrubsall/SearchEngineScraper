using System.Net;

namespace WebApp.Scrapers;

public class GoogleScraper : BaseScraper
{
    public GoogleScraper() : base(
        "https://www.google.co.uk/", 
        100,
        new List<string>() { "google" },
        "<a href=\"/url?q=",
        "\"")
    {
        
    }

    protected override List<Cookie> GetCookies()
    {
        return new List<Cookie>() { new("CONSENT", "YES+GB.en-GB+V9") };
    }

    protected override string GetQueryUrl(string searchQuery, int searchFrom)
    {
        return String.Format("https://www.google.co.uk/search?num={0}&q={1}&start={2}", this.maxSearches,
            searchQuery.Replace(' ', '+'), searchFrom);
    }
}