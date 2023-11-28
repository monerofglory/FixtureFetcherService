using Microsoft.AspNetCore.Mvc;

namespace FixtureFetcherService.Controllers
{
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
        [Route("{sportType}/team/{teamName}")]
        public Fixture? GetFixture(string sportType, string teamName)
        {
            DateOnly? date = GetDateFromUrlParameters();
            
            Fixture? result = null;
            switch (sportType)
            {
                case "soccer":
                    if (date != null)
                    {
                        result = f.GetFixtureByDate(teamName, date.Value);
                    }
                    else
                    {
                        result = f.GetNextFixture(teamName);
                    }
                    break;
            }

            if (result == null)
            {
                Response.StatusCode = 404;
            }
            return result;
        }

        private DateOnly? GetDateFromUrlParameters()
        {
            string date = HttpContext.Request.Query["date"].ToString();
            DateOnly? dateAsDateOnly = null;
            if (date != null && date != string.Empty)
            {
                switch (date)
                {
                    case "today":
                        dateAsDateOnly = DateOnly.FromDateTime(DateTime.Now);
                        break;
                    case "tomorrow":
                        dateAsDateOnly = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
                        break;
                    case "yesterday":
                        dateAsDateOnly = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
                        break;
                    default:
                        dateAsDateOnly = DateOnly.Parse(date);
                        break;
                }
            }
            return dateAsDateOnly;
        }
    }
}
