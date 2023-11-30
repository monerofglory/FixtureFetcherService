using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.Proxies;

namespace FixtureFetcherService.FixtureFetcherHelpers
{
    public interface ICalendarFetcher
    {
        public IUniqueComponentList<CalendarEvent> GetCalendarEvents(string sportType, string teamName);
    }
}
