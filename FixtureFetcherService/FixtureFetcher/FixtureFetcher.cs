using Ical.Net.CalendarComponents;
using Calendar = Ical.Net.Calendar;

namespace FixtureFetcherService
{
    public class FixtureFetcher
    {
        public Fixture? GetFixtureByDate(string sportType, string team, DateOnly date)
        {
            return GetFixture(sportType, team, date);
        }

        public Fixture? GetNextFixture(string sportType, string team)
        {
            return GetFixture(sportType, team);
        }

        private static async Task<Calendar> GetICalAsync(string url)
        {
            using var client = new HttpClient();
            using var stream = await client.GetStreamAsync(url);
            return Calendar.Load(stream);
        }

        public Fixture? GetFixture(string sportType, string team, DateOnly date)
        {
            Calendar cal = GetTeamCalendar(sportType, team);
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

        public Fixture? GetFixture(string sportType, string team)
        {
            Calendar teamCalendar = GetTeamCalendar(sportType, team);
            CalendarEvent? calendarEvent = teamCalendar.Events.OrderBy(x => x.DtStart).FirstOrDefault(x => x.DtStart.AsUtc >= DateTime.UtcNow);
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

        private static Calendar GetTeamCalendar(string sportType, string team)
        {
            try
            {
                return GetICalAsync($"https://sports.yahoo.com/{sportType}/teams/{team}/ical.ics").Result;
            }
            catch (Exception)
            {
                throw new FileNotFoundException($"Calendar file for {sportType} team {team} does not exist or is not supported.");
            }
        }
    }
}
