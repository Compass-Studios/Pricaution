namespace Pricaution.WebScraper.Parsers
{
	internal static class DataParser
	{
		public static string? Street(string input)
		{
			if (!input.Contains("ul."))
			{
				return null;
			}

			ushort start = (ushort)(input.IndexOf("ul.") + 4);
			string cropped = input.Substring(start);
			ushort end = cropped.Contains(',') ? (ushort)cropped.IndexOf(",") : (ushort)cropped.Length;

			return input.Substring(start, end);
		}
	}
}