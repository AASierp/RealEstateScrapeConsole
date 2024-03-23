using HtmlAgilityPack;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RealEstateScrapeConsole
{
    public class ClientRequest
    {
        private static readonly string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36";

        private static readonly HttpClient httpClient = new HttpClient();

        private static readonly Random rand = new Random();

        public static async Task<string> MakeHttpRequestAsync(string url)
        {             
            int randomDelayMiliseconds = rand.Next(100, 1000);

            await Task.Delay(randomDelayMiliseconds);
            
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            using HttpResponseMessage httpResponse = await httpClient.GetAsync(url);

            string htmlContent = await httpResponse.Content.ReadAsStringAsync();

            return htmlContent;
        }       
    }
}
