using AutoMapper;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services.APIs.HotelAPIs;

namespace GotorzProjectMain.Services.Mapping;

public class AmadeusMappingProfiles : Profile
{
    public AmadeusMappingProfiles()
    {
        CreateMap<GeoCodeDto, GeoCode>();
        CreateMap<DistanceDto, Distance>();

        CreateMap<HotelDto, Hotel>()
            .ForMember(dest => dest.IataCode, opt => opt.MapFrom(src => src.IataCode))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.DuplicateId, opt => opt.MapFrom(src => (int?)src.DupeId))
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom<AmadeusMapperAddressResolver>())
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.GeoCode))
            .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => src.Distance))
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Amenities))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.LastUpdate));
    }
}
