namespace FixtureFetcherService.FixtureFetcherHelpers
{
    public interface IFixtureFetcher
    {
        public Fixture? GetFixture(string sportType, string teamName);
        public Fixture? GetFixture(string sportType, string teamName, DateOnly date);
    }
}
