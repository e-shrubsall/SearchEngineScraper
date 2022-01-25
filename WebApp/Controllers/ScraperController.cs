using Microsoft.AspNetCore.Mvc;
using WebApp.Scrapers;

namespace WebApp.Controllers;

[Route("[controller]")]
public class ScraperController : Controller
{
    private Dictionary<SearchEngine, BaseScraper> scrapers { get; }

    public ScraperController()
    {
        this.scrapers = new Dictionary<SearchEngine, BaseScraper>();
        this.scrapers.Add(SearchEngine.Google, new GoogleScraper());
        this.scrapers.Add(SearchEngine.Bing, new BingScraper());
    }

    [Route("GetRanking")]
    [HttpPost]
    public async Task<ActionResult<int>> GetRanking([FromBody] QueryRequest queryRequest)
    {
        if (this.scrapers.TryGetValue(queryRequest.Engine, out var scraper))
        {
            return await scraper.GetRanking(queryRequest.QueryText, queryRequest.TargetUrl, 100);
        }
        throw new ArgumentException(String.Format("Could not find scraper for search engine: {0}", queryRequest.Engine));
    }
}