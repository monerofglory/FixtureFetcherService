using Ical.Net;

namespace FixtureFetcherService.FixtureFetcherHelpers
{
    public class CalendarFetcher : ICalendarFetcher
    {
        public Calendar GetCalendar(string sportType, string teamName)
        {
            try
            {
                return GetICalAsync($"https://sports.yahoo.com/{sportType}/teams/{teamName}/ical.ics").Result;
            }
            catch (Exception)
            {
                throw new FileNotFoundException($"Calendar file for {sportType} team {teamName} does not exist or is not supported.");
            }
        }
        private static async Task<Calendar> GetICalAsync(string url)
        {
            using var client = new HttpClient();
            using var stream = await client.GetStreamAsync(url);
            return Calendar.Load(stream);
        }
    }
}
