using GotorzProjectMain.InputModels.Users.CustomerInputModels;
using Microsoft.AspNetCore.Identity;

namespace GotorzProjectMain.Services;

public interface ICustomerRegistrationService
{
    Task<IEnumerable<IdentityError>?> RegisterAsync(RegisterCustomerInputModel Input);
}
