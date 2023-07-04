// Disable warnings about missing constructor
#pragma warning disable CS8618

namespace Pricaution.WebScraper.Models
{
	internal class OfferModel
	{
		public string Name { get; init; }
		public string Link { get; init; }
		public List<string> ImageUrls { get; init; }
		public string Address { get; init; }
		public string City { get; init; }
		public short Floor { get; init; } 
		public short MaxFloor { get; init; } 
		public uint Price { get; init; }
		public ushort Rooms { get; init; }
		public float Sq { get; init; }
		public ushort? Year { get; init; }
		public bool Elevator { get; init; }
	}

}