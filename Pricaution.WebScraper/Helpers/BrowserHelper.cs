using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace Pricaution.WebScraper.Helpers
{
	internal static class BrowserHelper
	{
		public static void SetupBrowser(out WebDriver driver, BrowserDriver browserChoice, bool headless)
		{
			switch (browserChoice)
			{
				case BrowserDriver.Edge:
				{
					driver = new EdgeDriver();
					if (!headless)
						driver.Manage().Window.Position = new Point(-2000, 0);
					break;
				}
				case BrowserDriver.Chrome:
				{
					driver = new ChromeDriver();
					if (!headless)
						driver.Manage().Window.Position = new Point(-2000, 0);
					break;
				}
				case BrowserDriver.Firefox:
				{
					driver = new FirefoxDriver();
					if (!headless)
						driver.Manage().Window.Position = new Point(-2000, 0);
					break;
				}
				default:
				{
					driver = new EdgeDriver();
					if (!headless)
						driver.Manage().Window.Position = new Point(-2000, 0);
					break;
				}
			}
		}
		
		public static void SetupMainPage(WebDriver driver)
		{
			try
			{
				string scrollAndRemoveScript = """
try {
	window.scrollBy(0, 100_000);
	document.querySelectorAll('.css-1dcvyuj')[0].remove();
} catch {

}
""";
				
				// Scroll to bottom to bypass lazy-loading & reemove "Promoted listings"
				driver.ExecuteScript(scrollAndRemoveScript, "");
			}
			catch
			{
				// ignored
			}
		}
		
		public static void SetupOfferPage(WebDriver driver)
		{
			// Scroll to bottom to bypass lazy-loading
			driver.ExecuteScript("window.scrollBy(0, 10_000)", "");

			// Remove svg pin icon from address
			// Has to be in try-catch, because it sometimes crashes(idk why)
			// After a few tests it turns out FindElement().Text ignores other tags, so removing the pin is unnecessary
			// felt cute, might delete later
			string removePinScript = """
try {
document.querySelectorAll('.fa-map-marker-alt')[0].remove();
} catch {

}
""";
			driver.ExecuteScript(removePinScript, "");
		}

		public static IWebElement FindElementByAriaLabel(this WebDriver driver, string label)
		{ 
			return driver.FindElement(By.CssSelector($"[aria-label=\"{label}\"]"));
		}
		public static IWebElement? TryFindElementByAriaLabel(this WebDriver driver, string label)
		{ 
			
			IWebElement? element;
			try
			{
				element = driver.FindElement(By.CssSelector($"[aria-label=\"{label}\"]"));
			}
			catch
			{
				element = null;
			}
			return element;
		}
		public static IWebElement? TryFindElement(this IWebElement driver, By by)
		{
			IWebElement? element;
			try
			{
				element = driver.FindElement(by);
			}
			catch
			{
				element = null;
			}
			return element;
		}
	}
}