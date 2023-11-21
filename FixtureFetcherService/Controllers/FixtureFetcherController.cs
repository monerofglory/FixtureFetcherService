using Microsoft.AspNetCore.Mvc;

namespace FixtureFetcherService.Controllers
{
    [Route("FixtureFetcher")]
    [ApiController]
    public class FixtureFetcherController : ControllerBase
    {
        private readonly ILogger<FixtureFetcherController> _logger;

        public FixtureFetcherController(ILogger<FixtureFetcherController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("GetFixtureByDate")]
        public Fixture GetFixtureByDate()
        {
            return new Fixture { AwayTeam = "Chelsea", HomeTeam = "Home" , KickOff = DateTime.Now};
        }

        [HttpGet]
        [Route("GetFixtureByDateRange")]
        public Fixture GetFixtureByDateRange()
        {
            return new Fixture { AwayTeam = "Chelsea", HomeTeam = "Home", KickOff = DateTime.Now };
        }
    }
}
