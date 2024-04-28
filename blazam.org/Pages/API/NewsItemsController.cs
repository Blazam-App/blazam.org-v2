using ApplicationNews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazamNews.Pages.API
{
    [Route("api/newsitems")]
    [ApiController]
    public class NewsItemsController : ControllerBase
    {
        private readonly IDbContextFactory<NewsDbContext> dbContextFactory;
        private NewsDbContext Context => dbContextFactory.CreateDbContext();
        public NewsItemsController(IDbContextFactory<NewsDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new ObjectResult(await Context.NewsItems.Take(50).ToListAsync());
        }

    }

}
