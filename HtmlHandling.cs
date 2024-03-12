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
            HtmlNodeCollection allPageLinks = htmlDocument.DocumentNode.SelectNodes("//div[@data-url]");

            //Isolates actual tag Value
            List<string> homeListingLinks = allPageLinks.Select(link => link.Attributes["data-url"].Value).ToList();

            //concatenates url to make a usable link.
            List<string> allListingUrls = homeListingLinks.Select(link => "https://www.joehaydenrealtor.com" + link).ToList();

            return allListingUrls;
        }

        public static PropertyModels ParseIndividualListingInfo(HtmlDocument htmlDocument)
        {
            List<HtmlNode> propertyInfoNodes = new List<HtmlNode>
        {
            htmlDocument.DocumentNode.SelectSingleNode("//div[@class='address']"),
            htmlDocument.DocumentNode.SelectSingleNode("//span[@class='si-ld-top__price']"),
            htmlDocument.DocumentNode.SelectSingleNode("//div[@class='si-idx-disclaimer']"),
            htmlDocument.DocumentNode.SelectSingleNode("//div[@class='si-ld-primary__info clearfix']"),
            htmlDocument.DocumentNode.SelectSingleNode("//div[@class='si-ld-primary__info clearfix']"),
            htmlDocument.DocumentNode.SelectSingleNode("//div[@class='si-ld-description js-listing-description']"),
            htmlDocument.DocumentNode.SelectSingleNode("//div[@class='si-ld-primary__info clearfix']")
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

            if (listingInfoTrimmed.Count >= 7)
            {
                propertyModel.Address = listingInfoTrimmed[0];
                propertyModel.Price = listingInfoTrimmed[1];
                propertyModel.DateListed = listingInfoTrimmed[2];
                string sqft = listingInfoTrimmed[3];
                string lot = listingInfoTrimmed[4];
                propertyModel.Description = listingInfoTrimmed[5];
                string county = listingInfoTrimmed[6];

                HtmlNode urlNode = htmlDocument.DocumentNode.SelectSingleNode("//link[@rel='canonical']");
                string url = urlNode.GetAttributeValue("href", "");

                propertyModel.Url = url;

                string targetWord = "Sq. Feet:";
                string pattern = $@"{Regex.Escape(targetWord)}\s*([^\s]+)";

                string targetWord2 = "Lot Size:";
                string pattern2 = $@"\b{Regex.Escape(targetWord2)}\s*([^\s]+)";

                string targetWord3 = "County:";
                string pattern3 = $@"{Regex.Escape(targetWord3)}\s*([^\s]+)";

                Match match = Regex.Match(sqft, pattern);
                sqft = match.Groups[1].Value;

                Match match2 = Regex.Match(lot, pattern2);
                lot = match2.Groups[1].Value;

                Match match3 = Regex.Match(county, pattern3);
                county = match3.Groups[1].Value;

                propertyModel.SquareFeet = sqft;
                propertyModel.LotSize = lot;
                propertyModel.County = county;
                propertyModel.Url = url;
            }
            return propertyModel;
        }
    }
}
