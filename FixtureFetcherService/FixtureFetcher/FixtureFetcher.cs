
using Ical.Net.CalendarComponents;
namespace FixtureFetcherService.FixtureFetcherHelpers
{
    public class FixtureFetcher : IFixtureFetcher
    {

        private readonly CalendarFetcher _calendarFetcher = new();
        public Fixture? GetFixture(string sportType, string teamName, DateOnly date)
        {
            CalendarEvent? calendarEvent = _calendarFetcher.GetCalendarEvents(sportType, teamName).FirstOrDefault(x => DateOnly.FromDateTime(x.Start.Date) == date);
            if (calendarEvent != null)
            {
                return CreateFixtureObject(calendarEvent);
            }
            return null;
            
        }


        public Fixture? GetFixture(string sportType, string teamName)
        {
            CalendarEvent? calendarEvent = _calendarFetcher.GetCalendarEvents(sportType, teamName).OrderBy(x => x.DtStart).FirstOrDefault(x => x.DtStart.AsUtc >= DateTime.UtcNow);
            if (calendarEvent != null)
            {
                return CreateFixtureObject(calendarEvent);
            }
            return null;
            
        }

        private static Fixture CreateFixtureObject(CalendarEvent calendarEvent)
        {
            var teams = calendarEvent.Summary.Split(" at ");

            return new Fixture
            {
                HomeTeam = teams.First().Replace("-", " "),
                AwayTeam = teams.Last().Replace("-", " "),
                KickOff = calendarEvent.DtStart.Value
            };
        }
    }
}
