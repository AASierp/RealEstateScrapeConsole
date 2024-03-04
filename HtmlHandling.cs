using HtmlAgilityPack;
using System.Net.Http;
using System.Text.RegularExpressions;


namespace RealEstateScrapeConsole
{
    public class HtmlHandling
    {
        public static HtmlDocument CreateHtmlDoc(string htmlContent)
        {
            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(htmlContent);

            return htmlDocument;
        }

        public static List<string> ParseHtmlForListingUrls(HtmlDocument htmlDocument)
        {

            //Selects nodes by Tag from HTML doc and adds them to a node list (in this case all the URLs).
            List<string> allPageLinks = htmlDocument.DocumentNode.SelectNodes("//a[@class='listing-price']")?.Select(a => a.GetAttributeValue("href", "")).ToList();


            //Takes top 10 listing links and concatenates them to produce a usable link.
            
            List<string> allListingUrls = allPageLinks.Select(link => "https://www.century21.com" + link).ToList();

            return allListingUrls;
        }

        public static PropertyModels ParseIndividualListingInfo(HtmlDocument htmlDocument)
        {
            List<HtmlNode> propertyInfoNodes = new List<HtmlNode>
        {
            htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='h3']"),
            htmlDocument.DocumentNode.SelectSingleNode("//h2[@class='property-price']"),
            htmlDocument.DocumentNode.SelectSingleNode("//span[@id='property-specs-sqft']"),
        };

            PropertyModels propertyModel = new PropertyModels();

            List<string> listingInfoTrimmed = new List<string>();

            foreach (HtmlNode node in propertyInfoNodes)
            {
                if (node != null)
                {
                    listingInfoTrimmed.Add(node.InnerText.Trim());
                }
            }

            string priceString = listingInfoTrimmed.Count > 1 ? listingInfoTrimmed[1] : "Price not found";
            string sqftString = listingInfoTrimmed.Count > 2 ? listingInfoTrimmed[2] : "Sqft not found";
            propertyModel.Address = listingInfoTrimmed.Count > 0 ? listingInfoTrimmed[0] : "Address not found";
            propertyModel.Price = int.Parse(priceString);
            propertyModel.SquareFeet = int.Parse(sqftString);
            

            return propertyModel;
        }
    }
}
