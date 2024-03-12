using HtmlAgilityPack;
using System;
using System.Diagnostics.Metrics;


namespace RealEstateScrapeConsole
{
    public class Scraper
    {
        public List<string> CompleteUrl(List<string> countyList)
        {
            List<string> completeCountyUrl = new List<string>();

            foreach (var county in countyList)
            {
                string countyUrl = $"https://www.joehaydenrealtor.com/{county}-county-ky/";

                completeCountyUrl.Add(countyUrl);

                string countyUrlSecondPage = $"https://www.joehaydenrealtor.com/{county}-county-ky/?pg=2";

                completeCountyUrl.Add(countyUrlSecondPage);

                string countyUrlThirdPage = $"https://www.joehaydenrealtor.com/{county}-county-ky/?pg=3";

                completeCountyUrl.Add(countyUrlThirdPage);
            }

            return completeCountyUrl;
        }
        public async Task<List<string>> ScrapeListingUrls(List<string> completeCountyUrl)
        {

            List<string> allListingUrls = new List<string>();

            foreach (string county in completeCountyUrl)
            {
                string htmlContent = await ClientRequest.MakeHttpRequestAsync(county);

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

                    Console.WriteLine(propertyModel.Address);

                    var existingEntry = propertyContext.PropertyModels.Any(x => x.Url == listingUrl);

                    if (!existingEntry)
                    {
                        propertyContext.PropertyModels.Add(propertyModel);
                    }

                    await propertyContext.SaveChangesAsync();
                }             
            }
        }
    }
}
