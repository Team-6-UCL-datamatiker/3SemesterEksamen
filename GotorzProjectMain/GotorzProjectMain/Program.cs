using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GotorzProjectMain.Components;
using GotorzProjectMain.Components.Account;
using GotorzProjectMain.Data;
using Microsoft.AspNetCore.ResponseCompression;
using GotorzProjectMain.Hubs;
using GotorzProjectMain.Services;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services.Mapping;
using GotorzProjectMain.Services.APIs.HotelAPIs;
using GotorzProjectMain.Services.APIs;
using GotorzProjectMain.Services.APIs.FlightAPI;

var builder = WebApplication.CreateBuilder(args);

// Load secrets from environment variables (for Amadeus API)
var apiKey = Environment.GetEnvironmentVariable("AMADEUS_API_KEY");
var apiSecret = Environment.GetEnvironmentVariable("AMADEUS_API_SECRET");
builder.Services.AddSingleton(new AmadeusSettings(apiKey, apiSecret));

// Add Blazor server & WASM support
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

// Register AutoMapper profiles
builder.Services.AddAutoMapper(typeof(UserMappingProfiles), typeof(VacationRequestMappingProfiles), typeof(AmadeusMappingProfiles));

// Load configuration files
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					 .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
					 .AddJsonFile("Connection.json", optional: false, reloadOnChange: true);

// Project-specific services
builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddScoped<IRateLimiter, RateLimiter>();
builder.Services.AddSingleton<ICityLookupService, CityLookupService>();
builder.Services.AddSingleton<IVRNotifierService, VRNotifierService>();

//Used for getting the user data everytime an employee or customer is loaded
builder.Services.AddScoped<IExtendedUserService, ExtendedUserService>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IVacationRequestTreeService, VacationRequestTreeService>();

// Service to handle hotel API
builder.Services.AddHttpClient<IAmadeusHotelAPIService, AmadeusHotelAPIService>(client =>
{
	client.BaseAddress = new Uri("https://api.amadeus.com/");
});

// Service to handle flight API
builder.Services.AddHttpClient<IFlightService, FlightService>(client =>
{
	client.BaseAddress = new Uri("https://serpapi.com/");
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();



builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = IdentityConstants.ApplicationScheme;
		options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	})
	.AddIdentityCookies();

// EF Core & Identity setup
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

// Use Gmail email sender in production
if (builder.Environment.IsDevelopment())
{
	builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
}
else
{
	builder.Services.AddScoped<IEmailSender<ApplicationUser>, GmailEmailSender>();
}


// Service to lockout users
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
	// Policy that allows both Admin and Employee roles
	options.AddPolicy("EmployeeOrAdmin", policy =>
		policy.RequireRole("Employee", "Admin"));

	// Policy that allows only Admin role
	options.AddPolicy("Admin", policy =>
		policy.RequireRole("Admin"));

	// Policy that allows only Admin role
	options.AddPolicy("Customer", policy =>
		policy.RequireRole("Customer"));
});

// Enable SignalR (for chat)
builder.Services.AddSignalR();

// Enable WebAssembly streaming compression
builder.Services.AddResponseCompression(opts =>
{
	opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
		["application/octet-stream"]);
});

var app = builder.Build();

app.UseResponseCompression();

// Apply migrations and seed admin account
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

	await DbInitializer.Execute(scope.ServiceProvider, context, userManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
    app.UseMigrationsEndPoint();
	app.UseMigrationsEndPoint();
	app.UseMigrationsEndPoint();
}

// Static files and routing
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAntiforgery();

// SignalR chat endpoint
app.MapHub<ChatHub>("/chathub");

// Razor components
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(GotorzProjectMain.Client._Imports).Assembly);

// Identity UI endpoints
app.MapAdditionalIdentityEndpoints();

app.Run();
