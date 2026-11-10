using Microsoft.VisualBasic;

namespace Url;

public class UrlReq
{
    public required string url { get; set; }
}


public class UrlRes
{
    public required int id { get; set; }
    public required string Url { get; set; }
    public required string shortCode { get; set; }
    public required DateTime createdAt { get; set; }
    public required DateTime updatedAt { get; set; }

}



public class UrlStatsRes
{
    public required int id { get; set; }
    public required string Url { get; set; }
    public required string shortCode { get; set; }
    public required DateTime createdAt { get; set; }
    public required DateTime updatedAt { get; set; }
    public int? accessCount { get; set; }

}