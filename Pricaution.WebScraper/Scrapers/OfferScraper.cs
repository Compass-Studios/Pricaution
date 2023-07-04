using OpenQA.Selenium;
using Pricaution.WebScraper.Helpers;
using Pricaution.WebScraper.Models;
using Pricaution.WebScraper.Parsers;
using Spectre.Console;

namespace Pricaution.WebScraper.Scrapers
{
	internal static class OfferScraper
	{
		private const string MainDetailClass = "css-1wi2w6s";
		private const string LocationTextClass = "css-1helwne";

		public static List<OfferModel> Scrape(WebDriver driver, List<string> listingLinks, CitySelectModel cityModel)
		{
			List<OfferModel> offers = new();
			ushort listingCount = (ushort)listingLinks.Count;

			AnsiConsole.Progress()
				.AutoRefresh(true)
				.HideCompleted(false)
				.Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new SpinnerColumn(Spinner.Known.Dots))
				.Start(ctx =>
				{
					ProgressTask task = ctx.AddTask("[green]Scraping offers[/]");

					for (ushort i = 0; i < listingCount; i++)
					{
						bool isLastOffer = (i - 1 == listingCount);

						OfferModel? offer = ScrapeOffer(driver, listingLinks[i], cityModel);
						if (offer is not null)
							offers.Add(offer);

						task.Increment(100d / listingLinks.Count);
					}
				});

			return offers;
		}

		private static OfferModel? ScrapeOffer(WebDriver driver, string url, CitySelectModel cityModel)
		{
			driver.Navigate().GoToUrl(url);

			BrowserHelper.SetupOfferPage(driver);

			try
			{
				//                     GETTERS
				string name = driver.FindElement(By.ClassName("css-1wnihf5")).Text; // Get offer title
				IWebElement imageCarousel = driver.FindElement(By.ClassName("image-gallery-thumbnails-container")); // Find image carousel
				IWebElement[] _images = imageCarousel.FindElements(By.TagName("img")).ToArray(); // Get images from carousel
				string _street = driver.FindElement(By.ClassName(LocationTextClass)).Text;
				string city = cityModel.ToString(); // Get city
				string _floor = driver.FindElementByAriaLabel("Piętro").FindElement(By.ClassName(MainDetailClass)).Text;
				string _price = driver.FindElementByAriaLabel("Cena").Text.Replace(" ", "");
				string _rooms = driver.FindElementByAriaLabel("Liczba pokoi").FindElement(By.ClassName(MainDetailClass)).Text;
				string _sq = driver.FindElementByAriaLabel("Powierzchnia").FindElement(By.ClassName(MainDetailClass)).Text;
				string? _year = driver.TryFindElementByAriaLabel("Rok budowy")?.TryFindElement(By.ClassName(MainDetailClass))?.Text;
				string? _elevator = driver.TryFindElementByAriaLabel("Winda")?.TryFindElement(By.ClassName(MainDetailClass))?.Text;

				//                     DEBUG PRINTS
				ArgumentHelper.RunIfUsed("debug", () =>
				{
					AnsiConsole.MarkupLine($"[yellow]{url}[/]");
					AnsiConsole.MarkupLine($"Floor: {_street}");
					AnsiConsole.MarkupLine($"Floor: {_floor}");
					AnsiConsole.MarkupLine($"Price: {_price}");
					AnsiConsole.MarkupLine($"Rooms: {_rooms}");
					AnsiConsole.MarkupLine($"Sq: {_sq}");
					AnsiConsole.MarkupLine($"Year: {_year}");
					AnsiConsole.MarkupLine($"Elevator: {_elevator}");
				});

				//                     PARSERS
				string? street = DataParser.Street(_street);
				if (street is null)
					return null;

				if (!DataParser.Floor(_floor, out short floor, out short maxFloor))
					return null;

				if (_price.Contains("Zapytajocenę"))
					return null;

				uint price = (uint)MathF.Floor(float.Parse(_price.Substring(0, _price.Length - 2)));
				if (price > 3_000_000)
					return null;

				List<string> images = new();
				foreach (IWebElement image in _images)
				{
					string imageSource = image.GetAttribute("src");
					imageSource = imageSource.Replace(";s=184x138;q=80", "");
					images.Add(imageSource);
				}

				ushort rooms = Convert.ToUInt16(_rooms);
				float.TryParse(_sq.Substring(0, _sq.Length - 3), out float sq);
				ushort? year = _year is not null ? Convert.ToUInt16(_year) : null;
				bool elevator = DataParser.Elevator(_elevator);

				return new()
				{
					Name = name,
					Link = url,
					ImageUrls = images,
					Address = street,
					City = city,
					Floor = floor,
					MaxFloor = maxFloor,
					Price = price,
					Rooms = rooms,
					Sq = sq,
					Year = year,
					Elevator = elevator
				};
			}
			catch (NoSuchElementException) { }
			catch (Exception e)
			{
				AnsiConsole.WriteException(e);
			}

			return null;
		}
	}
}