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
		// Maps from  modelclass BaseUser to the input model UserBaseInputModel
		CreateMap<BaseUser, UserBaseInputModel>()
			.ForPath(inputModel => inputModel.FirstName, opt => opt.MapFrom(user => user.User.FirstName))
			.ForPath(inputModel => inputModel.LastName, opt => opt.MapFrom(user => user.User.LastName))
			.ForPath(inputModel => inputModel.Email, opt => opt.MapFrom(user => user.User.Email))
			.ForMember(inputModel => inputModel.CustomUsername, opt => opt.MapFrom(user => user.CustomUserName))
			.ForPath(inputModel => inputModel.Phone, opt => opt.MapFrom(user => user.User.PhoneNumber));

		// Inherits BaseUser → UserBaseInputModel mapping
		CreateMap<Customer, CustomerBaseInputModel>()
			.IncludeBase<BaseUser, UserBaseInputModel>();

		// Maps from Employee → EmployeeBaseInputModel, including admin flag
		CreateMap<Employee, EmployeeBaseInputModel>()
			.ForMember(inputModel => inputModel.IsAdmin, opt => opt.MapFrom(user => user.IsAdmin))
			.IncludeBase<BaseUser, UserBaseInputModel>();

		// Maps back from UserBaseInputModel → BaseUser
		CreateMap<UserBaseInputModel, BaseUser>()
			.ForPath(user => user.User.FirstName, opt => opt.MapFrom(inputModel => inputModel.FirstName))
			.ForPath(user => user.User.LastName, opt => opt.MapFrom(inputModel => inputModel.LastName))
			.ForPath(user => user.User.Email, opt => opt.MapFrom(inputModel => inputModel.Email))
			.ForMember(user => user.CustomUserName, opt => opt.MapFrom(inputModel => inputModel.CustomUsername))
			.ForPath(user => user.User.PhoneNumber, opt => opt.MapFrom(inputModel => inputModel.Phone));

		// Inherits UserBaseInputModel → BaseUser mapping
		CreateMap<CustomerBaseInputModel, Customer>()
			.IncludeBase<UserBaseInputModel, BaseUser>();

		// Maps from EmployeeBaseInputModel → Employee, including admin flag
		CreateMap<EmployeeBaseInputModel, Employee>()
			.ForMember(user => user.IsAdmin, opt => opt.MapFrom(inputModel => inputModel.IsAdmin))
			.IncludeBase<UserBaseInputModel, BaseUser>();

		// Mapping used for new employee registrations
		CreateMap<RegisterEmployeeInputModel, Employee>()
			.ForMember(user => user.IsAdmin, opt => opt.MapFrom(inputModel => inputModel.IsAdmin))
			.IncludeBase<UserBaseInputModel, BaseUser>();
	}
}