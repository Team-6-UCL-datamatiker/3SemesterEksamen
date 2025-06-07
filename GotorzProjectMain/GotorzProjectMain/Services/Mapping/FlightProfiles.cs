using AutoMapper;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services.APIs.FlightAPI.DTO;
using System.Globalization;

namespace GotorzProjectMain.Services.Mapping
{
    public class FlightProfiles : Profile
    {
        public FlightProfiles() 
        {
			// Maps a single flight leg (DTO) into a Flight entity
			CreateMap<FlightLegDTO, Flight>()
                .ForMember(dest => dest.DepartureAirportCode, opt => opt.MapFrom(src => src.Departure.Id))
                .ForMember(dest => dest.DepartureAirportName, opt => opt.MapFrom(src => src.Departure.Name))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => ParseDate(src.Departure.Time)))
                .ForMember(dest => dest.ArrivalAirportCode, opt => opt.MapFrom(src => src.Arrival.Id))
                .ForMember(dest => dest.ArrivalAirportName, opt => opt.MapFrom(src => src.Arrival.Name))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => ParseDate(src.Arrival.Time)))
                .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.Airline));

			// Maps a full flight route (with multiple flights and layovers)
			CreateMap<FlightRouteDTO, FlightRoute>()
                .ForMember(dest => dest.Legs, opt => opt.MapFrom(src => src.Flights))
                .ForMember(dest => dest.Layovers, opt => opt.MapFrom(src => src.Layovers))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price));

			// Maps layover data between airports
			CreateMap<LayoverDTO, Layover>();
        }
        private static DateTime ParseDate(string isoDate)
        {
			// Parses ISO-8601 string into DateTime with timezone awareness
			return DateTime.Parse(isoDate, null, DateTimeStyles.RoundtripKind);
        }
    }
}
