using System.Globalization;
using AutoMapper;
using GotorzProjectMain.InputModels.Users;
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
            .ConstructUsing((src, context) => new Hotel(
                src.Name,
                src.IataCode,
                (int?)src.DupeId,
                src.HotelId,
                src.Address?.CountryCode,
                src.Amenities,
                src.Rating,
                src.LastUpdate,
                context.Mapper.Map<GeoCode?>(src.GeoCode),
                context.Mapper.Map<Distance?>(src.Distance)
            ));

        // Skal sættes op på en anden form når man arbejder med records frem for klasser fordi deres
        // properties er immutable når de først er sat - Så alle værdier her skal sættes gennem constructor.
        // (Medfører også at de kun kan bruges til at lave nye objekter.)

        CreateMap<RoomDto, RoomInfo>()
            .ConstructUsing(src => new RoomInfo(
                src.Type,
                src.TypeEstimated != null ? src.TypeEstimated.Category : null,
                src.TypeEstimated != null ? src.TypeEstimated.Beds : null,
                src.TypeEstimated != null ? src.TypeEstimated.BedType : null,
                src.Description != null ? src.Description.Text : null
            ));

        CreateMap<GuestsDto, GuestInfo>()
            .ConstructUsing(src => new GuestInfo(
                src.Adults
            ));

        CreateMap<PriceDto, PriceInfo>()
            .ConstructUsing(src => new PriceInfo(
                src.Currency,
                src.Total,
                (src.Variations != null && src.Variations.Average != null) ? src.Variations.Average.Total : null
            ));

        CreateMap<PoliciesDto, PolicySummary>()
            .ConstructUsing(src => new PolicySummary(
                src.PaymentType,
                src.Refundable != null ? src.Refundable.CancellationRefund : null,
                (src.Cancellations != null && src.Cancellations.Any()) ? src.Cancellations.First().Deadline : null
            ));

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

        // Specifik addresse ikke tilgængelig gennem API, kun geoCode...
        CreateMap<Hotel, HotelBooking>()
            .ForMember(hb => hb.HotelName, opt => opt.MapFrom(h => h.Name))
            .ForMember(hb => hb.Price, opt => opt.MapFrom(h => float.Parse(h.Offers!.First().Price!.TotalAmount!, CultureInfo.InvariantCulture)))
            .ForMember(hb => hb.Currency, opt => opt.MapFrom(h => h.Offers!.First().Price!.Currency))
            .ForMember(hb => hb.CheckInDate, opt => opt.MapFrom(h => DateTime.Parse(h.Offers!.First().CheckInDate!, CultureInfo.InvariantCulture)))
            .ForMember(hb => hb.CheckOutDate, opt => opt.MapFrom(h => DateTime.Parse(h.Offers!.First().CheckOutDate!, CultureInfo.InvariantCulture)))
            .ForMember(hb => hb.Adults, opt => opt.MapFrom(h => h.Offers!.First().Guests!.Adults))
            .ForMember(hb => hb.HotelRating, opt => opt.MapFrom(h => h.Rating))
            .ForMember(hb => hb.RoomDescription, opt => opt.MapFrom(h =>
                $"Category: {h.Offers!.First().Room!.Category}\n" +
                $"Type: {h.Offers!.First().Room!.Type}\n" +
                $"Beds: {h.Offers!.First().Room!.Beds} {h.Offers!.First().Room!.BedType}\n" +
                $"Description: {h.Offers!.First().Room!.DescriptionText}"))
            .ForMember(hb => hb.Misc, opt => opt.MapFrom(h => h.Amenities != null ? string.Join(", ", h.Amenities.Select(a => a)) : null))
            .ForMember(hb => hb.BoardType, opt => opt.MapFrom(h => h.Offers!.First().BoardType));
    }
}
