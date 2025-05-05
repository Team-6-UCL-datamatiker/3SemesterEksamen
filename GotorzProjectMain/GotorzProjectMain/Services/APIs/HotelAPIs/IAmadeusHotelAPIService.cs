using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;
public interface IAmadeusHotelAPIService
{
    Task<IEnumerable<Hotel>> SearchHotelsAsync(AmadeusHotelListParameters parameters);
    string BuildQuery(AmadeusHotelListParameters parameters);
}
