using Ical.Net;

namespace FixtureFetcherService.FixtureFetcherHelpers
{
    public interface ICalendarFetcher
    {
        public Calendar GetCalendar(string sportType, string teamName);
    }
}
