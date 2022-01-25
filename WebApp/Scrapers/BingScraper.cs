using System.Net;

namespace WebApp.Scrapers;

public class BingScraper : BaseScraper
{
    public BingScraper() : base(
        "https://www.bing.co.uk/", 
        50,
        new List<string>() { "bing" },
        "<a href=\"",
        "\"")
    {
        
    }

    protected override List<Cookie> GetCookies()
    {
        return new List<Cookie>() {};
    }

    protected override string GetQueryUrl(string searchQuery, int searchFrom)
    {
        return String.Format("https://www.bing.com/search?cc=gb&count={0}&q={1}&first={2}", this.maxSearches,
            searchQuery.Replace(' ', '+'), searchFrom);
    }
}