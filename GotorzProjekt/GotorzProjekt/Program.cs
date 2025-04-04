using GotorzProjekt.Client.Pages;
using GotorzProjekt.Components;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjekt;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents()
			.AddInteractiveWebAssemblyComponents();

		// Ensure the configuration file is being read correctly
		builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
							 .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
							 .AddJsonFile("Connection.json", optional: false, reloadOnChange: true);

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

		builder.Services.AddQuickGridEntityFrameworkAdapter();

		builder.Services.AddDatabaseDeveloperPageExceptionFilter();
		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
			app.UseMigrationsEndPoint();
		}

		app.UseHttpsRedirection();

		app.UseStaticFiles();
		app.UseAntiforgery();

		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode()
			.AddInteractiveWebAssemblyRenderMode()
			.AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

		app.Run();
	}
}

