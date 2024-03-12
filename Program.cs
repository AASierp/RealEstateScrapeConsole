namespace RealEstateScrapeConsole
{
    internal class Program
    {

        public static async Task Main(string[] args)
        {

            Scraper scraper = new Scraper();

                List<string> countyList = new List<string>()
                {
                    "Anderson", "Fayette", "Madison", "Clark", "Scott", "Montgomery",
                    "Jessamine", "Franklin", "Bourbon", "Mercer", "Garrard", "Estill", 
                    "Boyle", "Harrison", "Woodford"
                };

            List<string> completeCountyUrls = scraper.CompleteUrl(countyList);

            List<string> allListingUrls = await scraper.ScrapeListingUrls(completeCountyUrls);

            await scraper.ScrapeListingInfo(allListingUrls);
              
        }
    }
}
