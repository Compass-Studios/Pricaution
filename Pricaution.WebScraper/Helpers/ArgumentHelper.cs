using Pricaution.WebScraper.Parsers;

namespace Pricaution.WebScraper.Helpers
{
	internal static class ArgumentHelper
	{
		internal static void RunIfUsed(string argumentName, Action action, bool runCondition = true)
		{
			if (ArgumentParser.CheckIfUsed(argumentName) == runCondition)
				action();
		}
	}
}