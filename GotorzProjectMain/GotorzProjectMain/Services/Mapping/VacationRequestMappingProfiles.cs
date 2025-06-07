using AutoMapper;
using GotorzProjectMain.InputModels.Users;
using GotorzProjectMain.InputModels.VacationRequestInputModels;
using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services.Mapping;

public class VacationRequestMappingProfiles : Profile
{
	public VacationRequestMappingProfiles()
	{
		// Maps VacationRequest modelclass to input model used in forms/UI
		CreateMap<VacationRequest, VacationRequestBaseInputModel>()
		.ForMember(inputmodel => inputmodel.DepartureCity, opt => opt.MapFrom(request => request.DepartureCity))
		.ForMember(inputmodel => inputmodel.ArrivalCity, opt => opt.MapFrom(request => request.ArrivalCity))
		.ForMember(inputmodel => inputmodel.DepartureCountry, opt => opt.MapFrom(request => request.DepartureCountry))
		.ForMember(inputmodel => inputmodel.ArrivalCountry, opt => opt.MapFrom(request => request.ArrivalCountry))
		.ForMember(inputModel => inputModel.StartDate, opt => opt.MapFrom(request => request.StartDate))
		.ForMember(inputModel => inputModel.EndDate, opt => opt.MapFrom(request => request.EndDate))
		.ForMember(inputModel => inputModel.ChildrenAmount, opt => opt.MapFrom(request => request.ChildrenAmount))
		.ForMember(inputModel => inputModel.AdultsAmount, opt => opt.MapFrom(request => request.AdultsAmount))
		.ForMember(inputModel => inputModel.RoomsAmount, opt => opt.MapFrom(request => request.RoomsAmount))
		.ForMember(inputModel => inputModel.HotelRequest, opt => opt.MapFrom(request => request.HotelRequest))
		.ForMember(inputModel => inputModel.FlightRequest, opt => opt.MapFrom(request => request.FlightRequest))
		.ForMember(inputModel => inputModel.Misc, opt => opt.MapFrom(request => request.Misc));

		// Maps input model back to VacationRequest modelclass for saving to database
		CreateMap<VacationRequestBaseInputModel, VacationRequest>()
		.ForMember(request => request.DepartureCity, opt => opt.MapFrom(inputmodel => inputmodel.DepartureCity))
		.ForMember(request => request.ArrivalCity, opt => opt.MapFrom(inputmodel => inputmodel.ArrivalCity))
		.ForMember(request => request.DepartureCountry, opt => opt.MapFrom(inputmodel => inputmodel.DepartureCountry))
		.ForMember(request => request.ArrivalCountry, opt => opt.MapFrom(inputmodel => inputmodel.ArrivalCountry))
		.ForMember(request => request.StartDate, opt => opt.MapFrom(inputModel => inputModel.StartDate))
		.ForMember(request => request.EndDate, opt => opt.MapFrom(inputModel => inputModel.EndDate))
		.ForMember(request => request.ChildrenAmount, opt => opt.MapFrom(inputModel => inputModel.ChildrenAmount))
		.ForMember(request => request.AdultsAmount, opt => opt.MapFrom(inputModel => inputModel.AdultsAmount))
		.ForMember(request => request.RoomsAmount, opt => opt.MapFrom(inputModel => inputModel.RoomsAmount))
		.ForMember(request => request.HotelRequest, opt => opt.MapFrom(inputModel => inputModel.HotelRequest))
		.ForMember(request => request.FlightRequest, opt => opt.MapFrom(inputModel => inputModel.FlightRequest))
		.ForMember(request => request.Misc, opt => opt.MapFrom(inputModel => inputModel.Misc));
	}


}
