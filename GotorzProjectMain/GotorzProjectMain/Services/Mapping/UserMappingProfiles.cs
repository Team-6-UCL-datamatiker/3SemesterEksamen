using GotorzProjectMain.Models;
using AutoMapper;
using GotorzProjectMain.InputModels.Users;
using GotorzProjectMain.InputModels.Users.CustomerInputModels;

namespace GotorzProjectMain.Services.Mapping;

public class UserMappingProfiles : Profile
{
    public UserMappingProfiles()
    {
        CreateMap<UserBaseInputModel, BaseUser>()
            .ForMember(user => user.User.FirstName, opt => opt.MapFrom(inputModel => inputModel.FirstName))
            .ForMember(user => user.User.LastName, opt => opt.MapFrom(inputModel => inputModel.LastName))
            .ForMember(user => user.User.Email, opt => opt.MapFrom(inputModel => inputModel.Email))
            .ForMember(user => user.User.PhoneNumber, opt => opt.MapFrom(inputModel => inputModel.Phone));

        CreateMap<CustomerBaseInputModel, Customer>()
            .ForMember(user => user.CustomUserName, opt => opt.MapFrom(inputModel => inputModel.CustomUserName))
            .IncludeBase<UserBaseInputModel, BaseUser>();
    }
}