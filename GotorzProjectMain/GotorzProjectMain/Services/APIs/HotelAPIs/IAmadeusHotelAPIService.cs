using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;
public interface IAmadeusHotelAPIService
{
    public string? ErrorMessage { get; set; }
    public string? ApiResponseInfoMessage { get; set; }
    public IEnumerable<Hotel> Hotels { get; set; }
    public List<string> HotelIds { get; set; }

    Task<string> GetAccessTokenAsync();
    Task SearchHotelsAsync(AmadeusHotelListParameters listParameters, AmadeusHotelOffersParameters offersParameters);
    string BuildListQueryAsync(AmadeusHotelListParameters listParameters);
    string BuildOffersQueryAsync(AmadeusHotelOffersParameters parameters, List<string> hotelIds);
}
