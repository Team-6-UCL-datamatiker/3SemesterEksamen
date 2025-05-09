using AutoMapper;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services.APIs.HotelAPIs;

namespace GotorzProjectMain.Services.Mapping;

public class AmadeusMapperAddressResolver : IValueResolver<HotelDto, Hotel, string?>
{
    public string? Resolve(HotelDto source, Hotel destination, string? destMember, ResolutionContext context)
    {
        return source.Address?.CountryCode;
    }
}
