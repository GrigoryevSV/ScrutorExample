using System.ServiceModel.Syndication;
using System.Xml;
using Microsoft.Extensions.Caching.Memory;
using ScrutorExaple.Controllers;

namespace Scrutor.Controllers;

public class RssFeedReader : IRssFeedReader
{
    public RssItem GetItem(string slug)
    {
        var url = "https://www.code4it.dev/rss.xml";
        using var reader = XmlReader.Create(url);
        var feed = SyndicationFeed.Load(reader);

        if (feed == null) return null;
        
        SyndicationItem item = feed.Items.FirstOrDefault(item => item.Id.EndsWith(slug));
        if (item == null) return null;
        return new RssItem
        {
            Title = item.Title.Text,
            Url = item.Links.First().Uri.AbsoluteUri,
            Source = "RSS feed"
        };
    }
}

public class CachedFeedReader : IRssFeedReader
{
    private readonly IRssFeedReader _rssFeedReader;
    private readonly IMemoryCache _memoryCache;

    public CachedFeedReader(IRssFeedReader rssFeedReader, IMemoryCache memoryCache)
    {
        _rssFeedReader = rssFeedReader;
        _memoryCache = memoryCache;
    }

    public RssItem GetItem(string slug)
    {
        var isFromCache = _memoryCache.TryGetValue(slug, out RssItem item);
        if (!isFromCache)
        {
            item = _rssFeedReader.GetItem(slug);
        }
        else
        {
            item.Source = "Cache";
        }

        _memoryCache.Set(slug, item);
        return item;
    }
}