namespace Pricaution.WebScraper
{
	internal static class CitySelectParser
	{
		private const string BaseDomain = "https://www.otodom.pl/pl/oferty/sprzedaz";
		private const string UrlArgs = "&limit=72";

		public static string Parse(CitySelectModel model) => $"{BaseDomain}/mieszkanie/{ParseImpl(model)}?{UrlArgs}";

		private static string ParseImpl(CitySelectModel model) => model switch
		{
			CitySelectModel.Będzin => "bedzin",
			CitySelectModel.Białystok => "bialystok",
			CitySelectModel.Bydgoszcz => "bydgoszcz",
			CitySelectModel.DąbrowaGórnicza => "dabrowa-gornicza",
			CitySelectModel.Gdańsk => "gdansk",
			CitySelectModel.Katowice => "katowice",
			CitySelectModel.Kielce => "kielce",
			CitySelectModel.Kraków => "krakow",
			CitySelectModel.Łódź => "lodz",
			CitySelectModel.Poznań => "poznan",
			CitySelectModel.Rzeszów => "rzeszow",
			CitySelectModel.Szczecin => "szczecin",
			CitySelectModel.Toruń => "torun",
			CitySelectModel.Warszawa => "warszawa",
			CitySelectModel.Wrocław => "wroclaw"
		};
	}
}