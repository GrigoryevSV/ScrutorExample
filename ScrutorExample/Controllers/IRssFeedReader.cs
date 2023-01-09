namespace ScrutorExaple.Controllers
{
    public interface IRssFeedReader
    {
        RssItem GetItem(string slug);
    }
}