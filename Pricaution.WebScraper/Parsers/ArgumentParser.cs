namespace Pricaution.WebScraper.Parsers
{
	public static class ArgumentParser
	{
		private static Dictionary<string, string?> parsedArguments = new();
		internal static void ParseArguments(string[] args)
		{
			if (args.Length == 0)
				return;
			
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("--"))
				{
					string key = args[i].Substring(2);
	
					// Check if argument has a value
					if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
					{
						string value = args[i + 1];
	
						parsedArguments[key] = value;
	
						// Increment the index to skip the value in the next iteration
						i++;
					}
					else
						parsedArguments[key] = null;
				}
			}
		}

		internal static bool GetValue(string argumentName, out string value) => parsedArguments.TryGetValue(argumentName, out value);

		internal static bool CheckIfUsed(string argumentName) => parsedArguments.ContainsKey(argumentName);
	}
}