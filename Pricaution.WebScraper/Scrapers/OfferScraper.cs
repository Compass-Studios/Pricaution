using System.Drawing;
using OpenQA.Selenium;
using Pricaution.WebScraper.Helpers;
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
					ProgressTask task = ctx.AddTask("[green]Scraping listing links[/]");

					for (ushort i = 0; i < listingCount; i++)
					{
						bool isLastOffer = (i - 1 == listingCount);

						OfferModel? offer = ScrapeOffer(driver, listingLinks[i], cityModel);
						if (offer is not null)
							offers.Add(offer);

						task.Increment(100d / listingLinks.Count);

						if (!isLastOffer)
							Thread.Sleep(ArgumentParser.GetValue("threshold", out string value) ? Convert.ToInt32(value) : 5000);
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
				string name = driver.FindElement(By.ClassName("css-1wnihf5")).Text;
				IWebElement imageCarousel = driver.FindElement(By.ClassName("image-gallery-thumbnails-container"));
				IWebElement[] _images = imageCarousel.FindElements(By.TagName("img")).ToArray();
				List<string> images = new();
				foreach (IWebElement image in _images)
				{
					images.Add(image.GetAttribute("src"));
				}
				
				string _street = driver.FindElement(By.ClassName(LocationTextClass)).Text; // Get street
				string? street = DataParser.Street(_street); // Parse street
				if (street is null)
					return null;

				string city = cityModel.ToString();

				string _floor = driver.FindElementByAriaLabel("Piętro").FindElement(By.ClassName(MainDetailClass)).Text; // Get floor
				short floor;
				short maxFloor;
				switch (_floor.Split('/')[0])
				{
					case "sutenera":
					{
						floor = -1;
						break;
					}
					case "parter":
					{
						floor = 0;
						break;
					}
					default:
					{
						floor = Convert.ToInt16(_floor.Split('/')[0]);
						break;
					}
				}

				try
				{
					maxFloor = Convert.ToInt16(_floor.Split('/')[1]); // Parse max floor
				}
				catch
				{
					maxFloor = floor;
				}

				string _price = driver.FindElementByAriaLabel("Cena").Text.Replace(" ", ""); // Get price
				if (_price.Contains("Zapytajocenę"))
					return null;
				uint price = Convert.ToUInt32(_price.Substring(0, _price.Length - 2)); // Parse Price

				string _rooms = driver.FindElementByAriaLabel("Liczba pokoi").FindElement(By.ClassName(MainDetailClass)).Text; // Get Rooms
				ushort rooms = Convert.ToUInt16(_rooms); // Parse rooms

				string _sq = driver.FindElementByAriaLabel("Powierzchnia").FindElement(By.ClassName(MainDetailClass)).Text; // Get area
				float.TryParse(_sq.Substring(0, _sq.Length - 3), out float sq); // Parse area

				string? _year = driver.TryFindElementByAriaLabel("Rok budowy")?.TryFindElement(By.ClassName(MainDetailClass))?.Text; // Get year
				ushort? year = _year is not null ? Convert.ToUInt16(_year) : null; // Parse year

				string? _elevator = driver.TryFindElementByAriaLabel("Winda")?.TryFindElement(By.ClassName(MainDetailClass))?.Text; // Get elevator
				bool elevator = false; // Parse elevator
				switch (_elevator)
				{
					case "tak":
					{
						elevator = true;
						break;
					}
					case "nie":
					{
						elevator = false;
						break;
					}
					default:
					{
						elevator = false;
						break;
					}
				}

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
			catch (Exception e)
			{
				AnsiConsole.WriteException(e);
				driver.Manage().Window.Position = new Point(0, 0);
			}

			return null;
		}
	}
}