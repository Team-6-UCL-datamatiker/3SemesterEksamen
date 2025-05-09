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

        // Mapping from RoomDto (source API DTO) to RoomInfo (model record)
        CreateMap<RoomDto, RoomInfo>()
            .ConstructUsing(src => new RoomInfo(
                src.Type,
                src.TypeEstimated != null ? src.TypeEstimated.Category : null,
                src.TypeEstimated != null ? src.TypeEstimated.Beds : null,
                src.TypeEstimated != null ? src.TypeEstimated.BedType : null,
                src.Description != null ? src.Description.Text : null
            ));

        // Mapping from GuestsDto (source API DTO) to GuestInfo (model record)
        CreateMap<GuestsDto, GuestInfo>()
            .ConstructUsing(src => new GuestInfo(
                src.Adults
            ));

        // Mapping from PriceDto (source API DTO) to PriceInfo (model record)
        CreateMap<PriceDto, PriceInfo>()
            .ConstructUsing(src => new PriceInfo(
                src.Currency,
                src.Total,
                (src.Variations != null && src.Variations.Average != null) ? src.Variations.Average.Total : null
            ));

        // Mapping from PoliciesDto (source API DTO) to PolicySummary (model record)
        CreateMap<PoliciesDto, PolicySummary>()
            .ConstructUsing(src => new PolicySummary(
                src.PaymentType,
                (src.Refundable != null) ? src.Refundable.CancellationRefund : null,
                (src.Cancellations != null && src.Cancellations.Any()) ? src.Cancellations.First().Deadline : null
            ));
        
        // Mapping from OfferDto (source API DTO) to HotelOfferDetails (model record)
        CreateMap<OfferDto, HotelOffer>()
            .ConstructUsing((src, context) => new HotelOffer(
                src.Id,
                src.CheckInDate,
                src.CheckOutDate,
                src.RateCode,
                src.BoardType,
                context.Mapper.Map<RoomInfo?>(src.Room),
                context.Mapper.Map<GuestInfo?>(src.Guests),
                context.Mapper.Map<PriceInfo?>(src.Price),
                context.Mapper.Map<PolicySummary?>(src.Policies)
            ));
    }
}
