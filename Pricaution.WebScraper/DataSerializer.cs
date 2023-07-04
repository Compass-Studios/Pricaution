using Newtonsoft.Json;

namespace Pricaution.WebScraper
{
	internal static class DataSerializer
	{
		public static void SaveToFile(List<OfferModel> offers, CitySelectModel city)
		{
			string dir = Directory.GetCurrentDirectory();
			string file = Path.Combine(dir, $"{city.ToString()}.json");
			string json = JsonConvert.SerializeObject(offers, Formatting.Indented);
			File.WriteAllText(file, json);
		}
	}
}