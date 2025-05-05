using AutoMapper;
using GotorzProjectMain.InputModels.VacationOfferInputModels;
using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services.Mapping
{
    public class VacationOfferMappingProfiles : Profile
    {
        public VacationOfferMappingProfiles()
        {
            CreateMap<VacationOffer, VacationOfferBaseInputModel>()
                .ForMember(inputModel => inputModel.Country, opt => opt.MapFrom(offer => offer.Country))
                .ForMember(inputModel => inputModel.StartDate, opt => opt.MapFrom(offer => offer.StartDate))
                .ForMember(inputModel => inputModel.EndDate, opt => opt.MapFrom(offer => offer.EndDate))
                .ForMember(inputModel => inputModel.TotalPrice, opt => opt.MapFrom(offer => offer.TotalPrice))
                .ForMember(inputModel => inputModel.Misc, opt => opt.MapFrom(offer => offer.Misc))
                .ForMember(inputModel => inputModel.ExpirationDate, opt => opt.MapFrom(offer => offer.ExpirationDate));
            CreateMap<VacationOfferBaseInputModel, VacationOffer>()
                .ForMember(offer => offer.Country, opt => opt.MapFrom(inputModel => inputModel.Country))
                .ForMember(offer => offer.StartDate, opt => opt.MapFrom(inputModel => inputModel.StartDate))
                .ForMember(offer => offer.EndDate, opt => opt.MapFrom(inputModel => inputModel.EndDate))
                .ForMember(offer => offer.TotalPrice, opt => opt.MapFrom(inputModel => inputModel.TotalPrice))
                .ForMember(offer => offer.Misc, opt => opt.MapFrom(inputModel => inputModel.Misc))
                .ForMember(offer => offer.ExpirationDate, opt => opt.MapFrom(inputModel => inputModel.ExpirationDate));
        }
    }
   
}
