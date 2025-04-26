using System.Text;
using AutoMapper;
using GotorzProjectMain.Data;
using GotorzProjectMain.InputModels.Users.CustomerInputModels;
using GotorzProjectMain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace GotorzProjectMain.Services;

public class CustomerRegistrationService : ICustomerRegistrationService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerRegistrationService> _logger;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly string? _returnUrl;

    private const string CustomerRole = "Customer";
    private IEnumerable<IdentityError>? identityErrors;

    public CustomerRegistrationService(string? returnUrl, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ILogger<CustomerRegistrationService> logger, IUserStore<ApplicationUser> userStore)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _logger = logger;
        _userStore = userStore;
        _returnUrl = returnUrl;
    }

    public async Task<IEnumerable<IdentityError>?> RegisterAsync(RegisterCustomerInputModel input)
    {
        var user = new ApplicationUser();

        // UserStore = The object that actually knows how to create, read, update, delete users from your database.
        //await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);

        // Casts userstore to an IUserEmailStore<ApplicationUser>. Meaning: it forces the underlying database / user - store thing to support email operations like setting, getting, verifying emails for users.
        var emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
        await emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);

        // Giver identityErrors en værdi hvis metoden ikke returnerer succes. identityErrors bliver vist i browseren i så fald.
        identityErrors = identityErrors.Concat(await ExecuteAsync(() => _userManager.CreateAsync(user, input.Password)));
        identityErrors = identityErrors.Concat(await ExecuteAsync(() => _userManager.AddToRoleAsync(user, "Customer")));

        var customer = new Customer
        {
            Id = user.Id,
            User = user
        };

        customer = _mapper.Map<Customer>(input);

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        // Er det her noget vi bruger?
        _logger.LogInformation("User created a new account with password.");









        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = _navigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
            new Dictionary<string, object?> { ["userId"] = user.Id, ["code"] = code, ["returnUrl"] = ReturnUrl });






        user.UserName = input.CustomUserName;

        var create = await _userManager.CreateAsync(user, input.Password);
        if (!create.Succeeded)
            return (false, create.Errors.Select(e => e.Description).ToArray());

        if (!await _roleManager.RoleExistsAsync(CustomerRole))
            await _roleManager.CreateAsync(new IdentityRole(CustomerRole));

        await _userManager.AddToRoleAsync(user, CustomerRole);

        _logger.LogInformation("New customer {UserId} created.", user.Id);

        return (true, Array.Empty<string>());


    }

    public async Task<IEnumerable<IdentityError>> ExecuteAsync(Func<Task<IdentityResult>> action)
    {
        try
        {
            var result = await action();

            if (!result.Succeeded)
            {
                return result.Errors;
            }

            return Enumerable.Empty<IdentityError>();
        }
        catch (Exception)
        {
            throw;
        }
    }

}
