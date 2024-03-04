using HtmlAgilityPack;


namespace RealEstateScrapeConsole
{
    public class ScrapingLogic
    {
        public async Task<List<string>> ScrapeListingUrls(List<string> countyList)
        {
            List<string> allListingUrls = new List<string>();

            foreach (string county in countyList)
            {
                string countyUrl = $"https://www.century21.com/real-estate/{county}-county-ky/LNK{county}";

                string htmlContent = await ClientRequest.MakeHttpRequestAsync(countyUrl);

                HtmlDocument htmlDocument = HtmlHandling.CreateHtmlDoc(htmlContent);

                List<string> listingUrls = HtmlHandling.ParseHtmlForListingUrls(htmlDocument);

                allListingUrls.AddRange(listingUrls);
            }

            return allListingUrls;
        }

        public async Task ScrapeListingInfo(List<string> allListingUrls)
        {
            using (PropertyContext propertyContext = new PropertyContext())

            {
                foreach (string listingUrl in allListingUrls)
                {
                    string htmlContent = await ClientRequest.MakeHttpRequestAsync(listingUrl);

                    HtmlDocument htmlDocument = HtmlHandling.CreateHtmlDoc(htmlContent);

                    PropertyModels propertyModel = HtmlHandling.ParseIndividualListingInfo(htmlDocument);

                    propertyContext.PropertyModels.Add(propertyModel);
                }

                await propertyContext.SaveChangesAsync();
            }
        }
    }
}
