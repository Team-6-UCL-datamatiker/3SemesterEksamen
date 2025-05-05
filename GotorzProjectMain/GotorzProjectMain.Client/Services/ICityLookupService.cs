namespace GotorzProjectMain.Client.Services;

public interface ICityLookupService
{
    IEnumerable<string> Search(string input, int max = 10);
    (string CityCode, string AirportCode)? GetCodes(string label);
}
