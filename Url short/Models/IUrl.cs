namespace Url;

public interface IUrl
{
    Task<UrlRes> createUrl(string url);

     Task<UrlRes> updateUrl(string shortcode,string url);

     Task<bool> deleteUrl(string url);

    Task<UrlRes> getUrl(string shorturl);

    Task<UrlStatsRes> getStats(string shorturl);
}