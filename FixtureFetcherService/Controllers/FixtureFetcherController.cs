using Microsoft.AspNetCore.Mvc;

namespace FixtureFetcherService.Controllers
{
    [Route("FixtureFetcher")]
    [ApiController]
    public class FixtureFetcherController : ControllerBase
    {
        private readonly FixtureFetcherService f = new();
        private readonly ILogger<FixtureFetcherController> _logger;

        public FixtureFetcherController(ILogger<FixtureFetcherController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("GetTomorrowsFixture/{team}")]
        public Fixture? GetTomorrowsFixture(string team)
        {
            var result = f.GetFixtureByDate(team, DateOnly.FromDateTime(DateTime.Now).AddDays(-1));
            if (result == null)
            {
                Response.StatusCode = 404;
            }
            return result;
        }

        [HttpGet]
        [Route("GetTodaysFixture/{team}")]
        public Fixture? GetTodaysFixture(string team)
        {
            var result = f.GetFixtureByDate(team, DateOnly.FromDateTime(DateTime.Now));
            if (result == null)
            {
                Response.StatusCode = 404;
            }
            return result;
        }

        [HttpGet]
        [Route("GetNextFixture/{team}")]
        public Fixture? GetNextFixture(string team)
        {
            var result = f.GetNextFixture(team);
            if (result == null)
            {
                Response.StatusCode = 404;
            }
            return result;
        }


        [HttpGet]
        [Route("GetNextFixture/{team}/{date}")]
        public Fixture? GetNextFixture(string team, DateTime date)
        {
            var result = f.GetFixtureByDate(team, DateOnly.FromDateTime(date));
            if (result == null)
            {
                Response.StatusCode = 404;
            }
            return result;
        }
    }
}
