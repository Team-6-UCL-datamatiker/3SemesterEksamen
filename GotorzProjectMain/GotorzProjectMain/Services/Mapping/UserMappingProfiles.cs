using GotorzProjectMain.Models;
using AutoMapper;
using GotorzProjectMain.InputModels.Users;
using GotorzProjectMain.InputModels.Users.CustomerInputModels;

namespace GotorzProjectMain.Services.Mapping;

public class UserMappingProfiles : Profile
{
    public UserMappingProfiles()
    {
        CreateMap<BaseUser, UserBaseInputModel>()
            .ForPath(inputModel => inputModel.FirstName, opt => opt.MapFrom(user => user.User.FirstName))
            .ForPath(inputModel => inputModel.LastName, opt => opt.MapFrom(user => user.User.LastName))
            .ForPath(inputModel => inputModel.Email, opt => opt.MapFrom(user => user.User.Email))
            .ForPath(inputModel => inputModel.Phone, opt => opt.MapFrom(user => user.User.PhoneNumber));

        CreateMap<Customer, CustomerBaseInputModel>()
            .ForMember(inputModel => inputModel.CustomUserName, opt => opt.MapFrom(user => user.CustomUserName))
            .IncludeBase<BaseUser, UserBaseInputModel>();

        CreateMap<UserBaseInputModel, BaseUser>()
            .ForPath(user => user.User.FirstName, opt => opt.MapFrom(inputModel => inputModel.FirstName))
            .ForPath(user => user.User.LastName, opt => opt.MapFrom(inputModel => inputModel.LastName))
            .ForPath(user => user.User.Email, opt => opt.MapFrom(inputModel => inputModel.Email))
            .ForPath(user => user.User.PhoneNumber, opt => opt.MapFrom(inputModel => inputModel.Phone));

        CreateMap<CustomerBaseInputModel, Customer>()
            .ForMember(user => user.CustomUserName, opt => opt.MapFrom(inputModel => inputModel.CustomUserName))
            .IncludeBase<UserBaseInputModel, BaseUser>();
    }
}