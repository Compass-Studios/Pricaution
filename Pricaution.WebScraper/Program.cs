using OpenQA.Selenium;
using Pricaution.WebScraper.Helpers;
using Pricaution.WebScraper.Models;
using Pricaution.WebScraper.Parsers;
using Pricaution.WebScraper.Scrapers;
using Spectre.Console;
using Color = Spectre.Console.Color;

namespace Pricaution.WebScraper
{
	public class WebScraper
	{
		public static void Main(string[] args)
		{
			AnsiConsole.Write(new FigletText("Pricaution Web Scraper").Centered().Color(Color.DodgerBlue1));

			BrowserDriver browserChoice = BrowserDriver.Edge;

			// Get browser from arguments
			ArgumentParser.ParseArguments(args);
			if (ArgumentParser.GetValue("browser", out string browser))
				browserChoice = Enum.Parse<BrowserDriver>(browser);

			// If '--browser <name>' argument is not used, show prompt with available browsers 
			ArgumentHelper.RunIfUsed("browser", () =>
			{
				browserChoice = AnsiConsole.Prompt(
					new SelectionPrompt<BrowserDriver>()
						.Title("Select [green bold]browser[/] you want to use")
						.PageSize(3)
						.AddChoices(BrowserDriver.Chrome, BrowserDriver.Edge));
			}, false);

			// Get city to scrape
			CitySelectModel cityChoice;
			if (ArgumentParser.GetValue("city", out string _cityToScrape))
				cityChoice = Enum.Parse<CitySelectModel>(_cityToScrape);
			else
			{
				cityChoice = AnsiConsole.Prompt(
					new SelectionPrompt<CitySelectModel>()
						.Title("Select [green bold]city[/] you want to scrape")
						.PageSize(15)
						.AddChoices(CitySelectModel.Będzin, CitySelectModel.Białystok, CitySelectModel.Bydgoszcz, CitySelectModel.DąbrowaGórnicza, CitySelectModel.Gdańsk, CitySelectModel.Katowice, CitySelectModel.Kielce,
							CitySelectModel.Kraków, CitySelectModel.Łódź, CitySelectModel.Poznań, CitySelectModel.Rzeszów, CitySelectModel.Szczecin, CitySelectModel.Toruń, CitySelectModel.Warszawa, CitySelectModel.Wrocław));
			}

			// Check for headless mode
			bool headless = false;
			ArgumentHelper.RunIfUsed("headless", () => { headless = true; });
			if (headless is false)
				headless = AnsiConsole.Prompt(
					new SelectionPrompt<bool>()
						.Title("Do you want to run in headless mode?")
						.PageSize(3)
						.AddChoices(true, false)
						.UseConverter((choice) =>
						{
							if (choice)
								return "Yes (recommended)";

							return "No";
						}));

			BrowserHelper.SetupBrowser(out WebDriver driver, browserChoice, headless);

			List<string> listingLinks = MainPageScraper.Scrape(driver, CitySelectParser.Parse(cityChoice));

			driver.Quit();
			// What the actual FUCK. I don't know why results are duplicated, but Distinct() removes most of them*
			// *most of them - from my test it should return 656, but it returned 658
			// It kinda works, so don't touch it, I might try fix it in future
			listingLinks = listingLinks.Distinct().ToList();

			AnsiConsole.MarkupLine($"[green]Listings: {listingLinks.Count}[/]");
			
			BrowserHelper.SetupBrowser(out driver, browserChoice, headless);
			
			List<OfferModel> offers = OfferScraper.Scrape(driver, listingLinks, cityChoice);

			AnsiConsole.MarkupLine($"[green]Viable offers: {listingLinks.Count}[/]");
			AnsiConsole.MarkupLine($"[green]Saving file to json...[/]");
			DataSerializer.SaveToFile(offers, cityChoice);
			AnsiConsole.MarkupLine($"[green]Done![/]");

			Console.Beep();
			Console.Beep();
			Console.Beep();

			driver.Quit();
		}
	}
}