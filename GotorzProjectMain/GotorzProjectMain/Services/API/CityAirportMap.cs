namespace GotorzProjectMain.Services.API
{
	public static class CityAirportMap
	{
		public static readonly Dictionary<string, string[]> Map = new(StringComparer.OrdinalIgnoreCase)
		{
			["Copenhagen"] = new[] { "CPH", "RKE" },
			["London"] = new[] { "LHR", "LGW", "LCY", "STN", "LTN" },
			["Paris"] = new[] { "CDG", "ORY", "BVA" },
			["New York"] = new[] { "JFK", "EWR", "LGA" },
			["Los Angeles"] = new[] { "LAX", "BUR", "LGB", "SNA" },
			["Berlin"] = new[] { "BER", "SXF", "TXL" },
			["Madrid"] = new[] { "MAD", "BCN" },
			["Rome"] = new[] { "FCO", "CIA" },
			["Amsterdam"] = new[] { "AMS" },
			["Dubai"] = new[] { "DXB", "DWC" },
			["Tokyo"] = new[] { "HND", "NRT" },
			["Singapore"] = new[] { "SIN" },
			["Sydney"] = new[] { "SYD", "BNE" },
		};
	}
}
