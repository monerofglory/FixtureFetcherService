
using Ical.Net.CalendarComponents;
using Ical.Net;
namespace FixtureFetcherService.FixtureFetcherHelpers
{
    public class FixtureFetcher : IFixtureFetcher
    {

        private CalendarFetcher _calendarFetcher = new();
        public Fixture? GetFixture(string sportType, string teamName, DateOnly date)
        {
            Calendar cal = _calendarFetcher.GetCalendar(sportType, teamName);
            
            CalendarEvent? calendarEvent = cal.Events.FirstOrDefault(x => DateOnly.FromDateTime(x.Start.Date) == date);
            if (calendarEvent != null)
            {
                var teams = calendarEvent.Summary.Split(" at ");

                return new Fixture
                {
                    HomeTeam = teams.First().Replace("-", " "),
                    AwayTeam = teams.Last().Replace("-", " "),
                    KickOff = calendarEvent.DtStart.Value
                };
            }
            return null;
            
        }

        public Fixture? GetFixture(string sportType, string teamName)
        {
            Calendar cal = _calendarFetcher.GetCalendar(sportType, teamName);
            CalendarEvent? calendarEvent = cal.Events.OrderBy(x => x.DtStart).FirstOrDefault(x => x.DtStart.AsUtc >= DateTime.UtcNow);
            if (calendarEvent != null)
            {
                var teams = calendarEvent.Summary.Split(" at ");

                return new Fixture
                {
                    HomeTeam = teams.First().Replace("-", " "),
                    AwayTeam = teams.Last().Replace("-", " "),
                    KickOff = calendarEvent.DtStart.Value
                };
            }
            return null;
            
        }
    }
}
