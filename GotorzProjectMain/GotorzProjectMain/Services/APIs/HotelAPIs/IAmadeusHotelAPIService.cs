using GotorzProjectMain.InputModels.HotelAPIInputModels;
using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;
public interface IAmadeusHotelAPIService
{
    public string? ErrorMessage { get; set; }
    public string? ApiResponseInfoMessage { get; set; }
    public IEnumerable<Hotel> Hotels { get; set; }
    public List<string> HotelIds { get; set; }

    Task<string> GetAccessTokenAsync();
    Task SearchHotelsAsync(AmadeusHotelListInputModel listParameters, AmadeusHotelOfferInputModel offersParameters);
    string BuildListQueryAsync(AmadeusHotelListInputModel listParameters);
    string BuildOffersQueryAsync(AmadeusHotelOfferInputModel parameters, List<string> hotelIds);
}
