using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using Spectre.Console;
using Color = Spectre.Console.Color;

namespace Pricaution.WebScraper
{
	public class WebScraper
	{
		private static List<string> listingLinks = new();
		private const string baseUrl = "https://www.otodom.pl/pl/oferty/sprzedaz/mieszkanie/dabrowa-gornicza?distanceRadius=0&locations=%5Bcities_6-134%5D&limit=72";


		public static void Main()
		{
			AnsiConsole.Write(new FigletText("Pricaution Web Scraper").Centered().Color(Color.DodgerBlue1));

			SetupBrowser(out ChromeDriver driver, true);

			driver.Navigate().GoToUrl(baseUrl);

			// Get initial page info
			IWebElement[] pages = driver.FindElement(By.ClassName("css-geek62")).FindElements(By.TagName("button")).ToArray(); // Get bottom page navigation
			ushort pageCount = Convert.ToUInt16(pages[^2].Text); // Get amount of pages (from button that navigates to the last one)
			ushort listingCount = Convert.ToUInt16(driver.FindElement(By.ClassName("css-19fwpg")).Text); // Get listing count (global)
			ushort lastPageListingCount = (ushort)(listingCount % 72); // Get listing count (last page)

			AnsiConsole.Progress()
				.AutoRefresh(true)
				.HideCompleted(true)
				.Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn(Spinner.Known.Dots))
				.Start(ctx =>
				{
					ProgressTask task = ctx.AddTask("[green]Scraping listing links[/]");
					
					for (ushort i = 1; i <= pageCount; i++)
					{
						bool isLastPage = (i == pageCount);

						// Retry if rate limited (listings per page != scraped listing from current page)
						if (!ScrapeListings(driver, i, isLastPage ? lastPageListingCount : (ushort)72))
							i--;
						else
							task.Increment(isLastPage ? (100 / pageCount + 4) : (100 / pageCount));

						if (!isLastPage)
							Thread.Sleep(5_000);
					}
				});

			Console.ReadLine();
			driver.Quit();
		}

		private static void SetupBrowser(out ChromeDriver driver, bool headless = true)
		{
			ChromeOptions o = new();

			driver = new ChromeDriver(o);
			driver.CloseDevToolsSession();

			if (headless)
				driver.Manage().Window.Position = new Point(-2000, 0);
		}

		private static void SetupPage(ChromeDriver driver)
		{
			// Scroll to bottom to bypass lazy-loading
			driver.ExecuteScript("window.scrollBy(0, 100_000)", "");

			// Remove "Promoted listings"
			driver.ExecuteScript("document.querySelectorAll('.css-1dcvyuj')[0].remove()", "");
		}

		/// <summary>
		/// Scrapes current page to get links to individual listings
		/// </summary>
		/// <param name="driver">WebDriver</param>
		/// <param name="page">Page number to scrape</param>
		/// <param name="supposedListingCount">Supposed count of listing</param>
		/// <returns>If supposedListingCount was the same as amount of listings</returns>
		private static bool ScrapeListings(ChromeDriver driver, ushort page, ushort supposedListingCount)
		{
			driver.Navigate().GoToUrl($"{baseUrl}&page={page}");

			SetupPage(driver);

			ReadOnlyCollection<IWebElement> listings = driver.FindElements(By.ClassName("css-iq9jxc"));

			// Check if we are rate limited
			if (supposedListingCount != listings.Count)
				return false;

			foreach (IWebElement listing in listings)
			{
				string link = listing.FindElement(By.TagName("a")).GetAttribute("href");
				listingLinks.Add(link);
			}

			return true;
		}
	}
}