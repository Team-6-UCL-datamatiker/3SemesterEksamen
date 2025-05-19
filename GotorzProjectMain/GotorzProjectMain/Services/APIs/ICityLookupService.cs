namespace GotorzProjectMain.Services.APIs;

public interface ICityLookupService
{
    public List<string> Cities { get; init; }
	IEnumerable<string> SearchContains(string input, int max = 10);
	(string CityCode, string AirportCode)? GetCodes(string label);

}
