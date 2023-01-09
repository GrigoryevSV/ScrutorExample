using Microsoft.AspNetCore.Mvc;

namespace ScrutorExaple.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RssInfoController : ControllerBase
    {
        private readonly IRssFeedReader _rssFeedReader;

        public RssInfoController(IRssFeedReader rssFeedReader)
        {
            _rssFeedReader = rssFeedReader;
        }

        [HttpGet("{slug}")]
        public ActionResult<RssItem> GetBySlug(string slug)
        {
            var item = _rssFeedReader.GetItem(slug);

            if (item != null)
                return Ok(item);
            else return NotFound();
        }
    }
}