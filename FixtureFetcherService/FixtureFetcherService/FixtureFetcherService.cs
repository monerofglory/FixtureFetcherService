using Ical.Net.CalendarComponents;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Calendar = Ical.Net.Calendar;

namespace FixtureFetcherService
{
    public class FixtureFetcherService
    {
        public Fixture? GetFixtureByDate(string team, DateOnly date)
        {
            return GetFixture(team, date);
        }

        public Fixture? GetNextFixture(string team)
        {
            return GetFixture(team);
        }

        private static async Task<Calendar> GetICalAsync(string url)
        {
            using var client = new HttpClient();
            using var stream = await client.GetStreamAsync(url);
            return Calendar.Load(stream);
        }

        public Fixture? GetFixture(string team, DateOnly date)
        {
            Calendar cal = GetTeamCalendar(team);
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

        public Fixture? GetFixture(string team)
        {
            Calendar teamCalendar = GetTeamCalendar(team);
            CalendarEvent? calendarEvent = teamCalendar.Events.FirstOrDefault(x => x.DtStart.AsUtc >= DateTime.UtcNow);
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

        private static Calendar GetTeamCalendar(string team)
        {
            try
            {
                return GetICalAsync($"https://sports.yahoo.com/soccer/teams/{team}/ical.ics").Result;
            }
            catch (Exception)
            {
                throw new FileNotFoundException($"Calendar file for {team} does not exist or is not supported.");
            }
        }
    }
}
