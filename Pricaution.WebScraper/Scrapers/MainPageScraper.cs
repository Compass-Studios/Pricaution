using System.Collections.ObjectModel;
using OpenQA.Selenium;
using Pricaution.WebScraper.Helpers;
using Spectre.Console;

namespace Pricaution.WebScraper.Scrapers
{
	internal static class MainPageScraper
	{
		private static List<string> listingLinks = new();
		private static ushort ListingCount { get; set; }
		
		public static List<string> Scrape(WebDriver driver, string url)
		{
			driver.Navigate().GoToUrl(url);
			
			GetMainPageInfo(driver, out ushort pageCount, out ushort lastPageListingCount);
			
			AnsiConsole.Progress()
				.AutoRefresh(true)
				.HideCompleted(false)
				.Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn(Spinner.Known.Dots))
				.Start(ctx =>
				{
					ProgressTask task = ctx.AddTask("[green]Scraping listing links[/]");

					for (ushort i = 1; i <= pageCount; i++)
					{
						bool isLastPage = (i == pageCount);

						// Retry if rate limited (listings per page != scraped listing from current page)
						if (!ScrapeListings(driver, url, i, isLastPage ? lastPageListingCount : (ushort)72))
							i--;
						else
							task.Increment(100d / pageCount);
					}
				});

			return listingLinks;
		}
		
		private static void GetMainPageInfo(WebDriver driver, out ushort pageCount, out ushort lastPageListingCount)
		{
			// Get initial page info
			IWebElement[] pages = driver.FindElement(By.ClassName("css-geek62")).FindElements(By.TagName("button")).ToArray(); // Get bottom page navigation
			pageCount = Convert.ToUInt16(pages[^2].Text); // Get amount of pages (from button that navigates to the last one)
			ListingCount = Convert.ToUInt16(driver.FindElement(By.ClassName("css-19fwpg")).Text); // Get listing count (global)
			lastPageListingCount = (ushort)(ListingCount % 72); // Get listing count (last page)
		}
		
		/// <summary>
		/// Scrapes current page to get links to individual listings
		/// </summary>
		/// <param name="driver">WebDriver</param>
		/// <param name="page">Page number to scrape</param>
		/// <param name="supposedListingCount">Supposed count of listing</param>
		/// <returns>If supposedListingCount was the same as amount of listings</returns>
		private static bool ScrapeListings(WebDriver driver, string url, ushort page, ushort supposedListingCount)
		{
			driver.Navigate().GoToUrl($"{url}&page={page}");

			BrowserHelper.SetupMainPage(driver);

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