using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;
public interface IAmadeusHotelAPIService
{
    Task<IEnumerable<Hotel>> SearchHotelsAsync(AmadeusHotelListParameters parameters, AmadeusHotelSearchParameters searchParameters);
    string BuildListQuery(AmadeusHotelListParameters parameters);
    string BuildSearchQuery(AmadeusHotelSearchParameters parameters, List<string> hotelIds);
    Task<string> GetAccessTokenAsync();
}
