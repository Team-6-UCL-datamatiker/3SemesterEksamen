using AutoMapper;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services.APIs.HotelAPIs.DTOs;

namespace GotorzProjectMain.Services.APIs.HotelAPIs;

public class AmadeusMapperAddressResolver : IValueResolver<HotelDto, Hotel, string?>
{
    public string? Resolve(HotelDto source, Hotel destination, string? destMember, ResolutionContext context)
    {
        return source.Address?.CountryCode;
    }
}
