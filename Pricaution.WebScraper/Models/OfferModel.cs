namespace Pricaution.WebScraper
{
	internal class OfferModel
	{
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