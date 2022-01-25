using System.Net;

namespace WebApp.Scrapers;

public abstract class BaseScraper
{
    protected string url { get; }
    protected int maxSearches { get; }
    protected List<string> excludeList { get; }
    protected string searchStartPhrase { get; }
    protected string searchEndPhrase { get; }
    
    protected BaseScraper(string url, int maxSearches, List<string> excludeList, string searchStartPhrase, string searchEndPhrase)
    {
        this.url = url;
        this.maxSearches = maxSearches;
        this.excludeList = excludeList;
        this.searchStartPhrase = searchStartPhrase;
        this.searchEndPhrase = searchEndPhrase;
    }
    
    /*
     * Get the ranking from the search query. Can call it multiple times if it doesn't get enough results or the
     * search engine has limitations.
     * Returns a number 1-101 where 101 is 101 or more
     */
    public async Task<int> GetRanking(string searchQuery, string targetUrl, int resultsCount)
    {
        var cookieContainer = new CookieContainer();
        var uri = new Uri(this.url);
        this.GetCookies().ForEach(cookie => cookieContainer.Add(uri, cookie));

        var urlList = new List<string>();
        
        using (var httpHandler = new HttpClientHandler() { CookieContainer = cookieContainer })
        using (var httpClient = new HttpClient(httpHandler))
        {
            // Split the query across multiple pages as necessary
            while (urlList.Count < resultsCount)
            {
                var response = await httpClient.GetAsync(this.GetQueryUrl(searchQuery, urlList.Count));
                var html = await response.Content.ReadAsStringAsync();
                if (ScrapeHtml(urlList, html, targetUrl, resultsCount))
                {
                    return urlList.Count;
                }
            }
        }
        return resultsCount + 1;
    }

    /*
     * Scrape through the html looking for search entries defined for each search engine.
     * Keeps track of where is is currently searching
     * Returns true if it finds the target and false otherwise.
     */
    private bool ScrapeHtml(List<String> resultUrls, string html, string targetUrl, int resultsCount)
    {
        var currentIndex = 0;
        var previousUrl = "";
        while ((currentIndex = html.IndexOf(this.searchStartPhrase, currentIndex)) > 0 &&
               resultUrls.Count < resultsCount)
        {
            currentIndex += this.searchStartPhrase.Length;
            var endIndex = html.IndexOf(this.searchEndPhrase, currentIndex);
            var resultUrl = this.StripUrl(html.Substring(currentIndex, endIndex - currentIndex));
            if (!this.excludeList.Any(exclude => resultUrl.Contains(exclude)))
            {
                if (resultUrl != previousUrl)
                {
                    resultUrls.Add(resultUrl);
                    if (resultUrl == targetUrl)
                    {
                        return true;
                    }
                }
            }
            previousUrl = resultUrl;
            currentIndex = endIndex + this.searchEndPhrase.Length;
        }
        return false;
    }

    /*
     * Strip the websites URL so that instead of 'https://www.infotrack.co.uk/...' you get 'www.infotrack.co.uk'
     */
    private string StripUrl(string websiteUrl)
    {
        var doubleSlashIndex = websiteUrl.IndexOf("//") + 2;
        var endSlashIndex = websiteUrl.IndexOf("/", doubleSlashIndex) - doubleSlashIndex;
        return websiteUrl.Substring(doubleSlashIndex, 
            endSlashIndex < 0 ? websiteUrl.Length - doubleSlashIndex : endSlashIndex);
    }

    /*
     * Construct the search query depending on the search engine used
     */
    protected abstract string GetQueryUrl(string searchQuery, int searchFrom);

    protected abstract List<Cookie> GetCookies();
}