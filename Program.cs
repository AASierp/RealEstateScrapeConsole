using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RealEstateScrapeConsole
{
	internal class Program
	{
		public static async Task Main(string[] args)
		{

			List<string> countyList = new List<string>()
				{
					"anderson", "bourbon", "boyle", "clark", "estill", "fayette", "franklin", "garrard",
					"harrison", "jessamine", "madison", "mercer", "montgomery", "scott", "woodford"
				};

			List<string> userCounties = new List<string>();

			Console.WriteLine("Select an option from the following menu.\n");
			Console.WriteLine("Press 1 to choose which counties to scrape.\n");
			Console.WriteLine("Press 2 to scrape the entire list of counties.\n");

			string userInput = Console.ReadLine();

			if (int.TryParse(userInput, out int option))
			{
				switch (option)
				{
					case 1:
						await UserSelectedCounties(countyList, userCounties);
						break;
					case 2:
						await ScrapeAllCounties(countyList);
						break;
				}
			}
			else
			{
				Console.WriteLine("That is not a valid selection, please try again.\n");
			}
		}

		private static async Task UserSelectedCounties(List<string> countyList, List<string> userCounties)
		{
			string userSelection;

			Console.WriteLine("Here is a list of the availble counties. Please type the name of the county to add it to your list.\n" +
					"When you are finished, type RUN to start scraping.\n");

			foreach (var i in countyList)
			{
				Console.WriteLine(i);
			}
				Console.WriteLine("\n");
			do
			{

				userSelection = Console.ReadLine().ToLower();

				if (countyList.Contains(userSelection))
				{
					userCounties.Add(userSelection);

					Console.WriteLine($"{userSelection} has been added to the list.\n");
				}
				else if (userSelection.ToLower() == "run")
				{
					Console.WriteLine("Scraper Initializing, Please Wait...\n");
				}
				else
				{
					if (!countyList.Contains(userSelection))

					Console.WriteLine("That is not a valid Selection.\n");
				}

			} while (userSelection.ToLower() != "run");

			Scraper scraper = new Scraper();

			List<string> completeCountyUrls = scraper.CompleteUrl(userCounties);

			List<string> allListingUrls = await scraper.ScrapeListingUrls(completeCountyUrls);

			await scraper.ScrapeListingInfo(allListingUrls);
		}

		private static async Task ScrapeAllCounties(List<string> countyList)
		{
			Console.WriteLine("Scraper Initializing, Please Wait...\n");

			Scraper scraper = new Scraper();

			List<string> completeCountyUrls = scraper.CompleteUrl(countyList);

			List<string> allListingUrls = await scraper.ScrapeListingUrls(completeCountyUrls);

			await scraper.ScrapeListingInfo(allListingUrls);
		}
	}
}
			
			
				

		

