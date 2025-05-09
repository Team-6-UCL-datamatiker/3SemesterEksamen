namespace GotorzProjectMain.Services.APIs;

public interface ICityLookupService
{
    IEnumerable<string> Search(string input, int max = 10);
    (string CityCode, string AirportCode)? GetCodes(string label);
}
