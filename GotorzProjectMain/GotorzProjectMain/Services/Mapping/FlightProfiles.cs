using AutoMapper;
using GotorzProjectMain.Models;
using GotorzProjectMain.Models.DTOs;
using System.Globalization;

namespace GotorzProjectMain.Services.Mapping
{
    public class FlightProfiles : Profile
    {
        public FlightProfiles() 
        {
            CreateMap<FlightLegDTO, Flight>()
                .ForMember(dest => dest.DepartureAirportCode, opt => opt.MapFrom(src => src.Departure.Id))
                .ForMember(dest => dest.DepartureAirportName, opt => opt.MapFrom(src => src.Departure.Name))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => ParseDate(src.Departure.Time)))
                .ForMember(dest => dest.ArrivalAirportCode, opt => opt.MapFrom(src => src.Arrival.Id))
                .ForMember(dest => dest.ArrivalAirportName, opt => opt.MapFrom(src => src.Arrival.Name))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => ParseDate(src.Arrival.Time)))
                .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.Airline));

            // Map FlightRouteDTO → FlightRoute
            CreateMap<FlightRouteDTO, FlightRoute>()
                .ForMember(dest => dest.Legs, opt => opt.MapFrom(src => src.Flights))
                .ForMember(dest => dest.Layovers, opt => opt.MapFrom(src => src.Layovers))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price));

            CreateMap<LayoverDTO, Layover>();
        }
        private static DateTime ParseDate(string isoDate)
        {
            // Helper to parse ISO-8601 datetime strings safely
            return DateTime.Parse(isoDate, null, DateTimeStyles.RoundtripKind);
        }
    }
}
