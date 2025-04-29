using AutoMapper;
using GotorzProjectMain.InputModels.Users;
using GotorzProjectMain.InputModels.VacationRequestInputModels;
using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services.Mapping;

public class VacationRequestMappingProfiles : Profile
{
    public VacationRequestMappingProfiles()
    {
        CreateMap<VacationRequest, VacationRequestBaseInputModel>()
        .ForMember(inputModel => inputModel.Country, opt => opt.MapFrom(user => user.Country))
        .ForMember(inputModel => inputModel.StartDate, opt => opt.MapFrom(user => user.StartDate))
        .ForMember(inputModel => inputModel.EndDate, opt => opt.MapFrom(user => user.EndDate))
        .ForMember(inputModel => inputModel.ChildrenAmount, opt => opt.MapFrom(user => user.ChildrenAmount))
        .ForMember(inputModel => inputModel.AdultsAmount, opt => opt.MapFrom(user => user.AdultsAmount))
        .ForMember(inputModel => inputModel.RoomsAmount, opt => opt.MapFrom(user => user.RoomsAmount))
        .ForMember(inputModel => inputModel.HotelRequest, opt => opt.MapFrom(user => user.HotelRequest))
        .ForMember(inputModel => inputModel.FlightRequest, opt => opt.MapFrom(user => user.FlightRequest))
        .ForMember(inputModel => inputModel.Misc, opt => opt.MapFrom(user => user.Misc));

        CreateMap<VacationRequestBaseInputModel, VacationRequest>()
        .ForMember(user => user.Country, opt => opt.MapFrom(inputModel => inputModel.Country))
        .ForMember(user => user.StartDate, opt => opt.MapFrom(inputModel => inputModel.StartDate))
        .ForMember(user => user.EndDate, opt => opt.MapFrom(inputModel => inputModel.EndDate))
        .ForMember(user => user.ChildrenAmount, opt => opt.MapFrom(inputModel => inputModel.ChildrenAmount))
        .ForMember(user => user.AdultsAmount, opt => opt.MapFrom(inputModel => inputModel.AdultsAmount))
        .ForMember(user => user.RoomsAmount, opt => opt.MapFrom(inputModel => inputModel.RoomsAmount))
        .ForMember(user => user.HotelRequest, opt => opt.MapFrom(inputModel => inputModel.HotelRequest))
        .ForMember(user => user.FlightRequest, opt => opt.MapFrom(inputModel => inputModel.FlightRequest))
        .ForMember(user => user.Misc, opt => opt.MapFrom(inputModel => inputModel.Misc));
    }


}
