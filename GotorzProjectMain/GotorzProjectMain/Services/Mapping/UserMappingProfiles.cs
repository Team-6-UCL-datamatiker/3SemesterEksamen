using GotorzProjectMain.Models;
using AutoMapper;
using GotorzProjectMain.InputModels.Users;
using GotorzProjectMain.InputModels.Users.CustomerInputModels;
using GotorzProjectMain.InputModels.Users.EmployeeInputModels;

namespace GotorzProjectMain.Services.Mapping;

public class UserMappingProfiles : Profile
{
	public UserMappingProfiles()
	{
		CreateMap<BaseUser, UserBaseInputModel>()
			.ForPath(inputModel => inputModel.FirstName, opt => opt.MapFrom(user => user.User.FirstName))
			.ForPath(inputModel => inputModel.LastName, opt => opt.MapFrom(user => user.User.LastName))
			.ForPath(inputModel => inputModel.Email, opt => opt.MapFrom(user => user.User.Email))
			.ForMember(inputModel => inputModel.CustomUsername, opt => opt.MapFrom(user => user.CustomUserName))
			.ForPath(inputModel => inputModel.Phone, opt => opt.MapFrom(user => user.User.PhoneNumber));

		CreateMap<Customer, CustomerBaseInputModel>()
			.IncludeBase<BaseUser, UserBaseInputModel>();

		CreateMap<Employee, EmployeeBaseInputModel>()
			.ForMember(inputModel => inputModel.IsAdmin, opt => opt.MapFrom(user => user.IsAdmin))
			.IncludeBase<BaseUser, UserBaseInputModel>();

		CreateMap<UserBaseInputModel, BaseUser>()
			.ForPath(user => user.User.FirstName, opt => opt.MapFrom(inputModel => inputModel.FirstName))
			.ForPath(user => user.User.LastName, opt => opt.MapFrom(inputModel => inputModel.LastName))
			.ForPath(user => user.User.Email, opt => opt.MapFrom(inputModel => inputModel.Email))
			.ForMember(user => user.CustomUserName, opt => opt.MapFrom(inputModel => inputModel.CustomUsername))
			.ForPath(user => user.User.PhoneNumber, opt => opt.MapFrom(inputModel => inputModel.Phone));

		CreateMap<CustomerBaseInputModel, Customer>()
			.IncludeBase<UserBaseInputModel, BaseUser>();

		CreateMap<EmployeeBaseInputModel, Employee>()
			.ForMember(user => user.IsAdmin, opt => opt.MapFrom(inputModel => inputModel.IsAdmin))
			.IncludeBase<UserBaseInputModel, BaseUser>();

		CreateMap<RegisterEmployeeInputModel, Employee>()
			.ForMember(user => user.IsAdmin, opt => opt.MapFrom(inputModel => inputModel.IsAdmin))
			//.ForMember(user => user.ProfilePicture, opt => opt.MapFrom(inputModel => inputModel.ProfilePictureFile))
			.IncludeBase<UserBaseInputModel, BaseUser>();
	}
}