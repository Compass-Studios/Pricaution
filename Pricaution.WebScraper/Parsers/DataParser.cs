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

		public static bool Floor(string input, out short floor, out short maxFloor)
		{
			floor = 0;
			maxFloor = 0;
			
			try
			{
				switch (input.Split('/')[0])
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
					case "poddasze":
					{
						try
						{
							floor = Convert.ToInt16(input.Split('/')[1]);
						}
						catch
						{
							return false;
						}
						break;
					}
					default:
					{
						floor = Convert.ToInt16(input.Split('/')[0]);
						break;
					}
				}

				try
				{
					maxFloor = Convert.ToInt16(input.Split('/')[1]); // Parse max floor
				}
				catch
				{
					maxFloor = floor;
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool Elevator(string input)
		{
			switch (input)
			{
				case "tak":
				{
					return true;
				}
				case "nie":
				{
					return false;
				}
				default:
				{
					return false;
				}
			}
		}
	}
}