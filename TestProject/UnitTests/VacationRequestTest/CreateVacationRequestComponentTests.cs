
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using GotorzProjectMain.Components.Pages.VacationRequestPages;
using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestProject;

[TestClass]
public class CreateVacationRequestTests : Bunit.TestContext
{
	private Mock<IDbContextFactory<ApplicationDbContext>> mockDbFactory;
	private Mock<IHubContext<GotorzProjectMain.Hubs.VacationRequestHub>> mockHubContext;
	private Mock<IVacationRequestSignalRService> mockSignalRService;

	[TestInitialize]
	public void Setup()
	{
		// Set up mocks
		mockDbFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
		mockHubContext = new Mock<IHubContext<GotorzProjectMain.Hubs.VacationRequestHub>>();
		mockSignalRService = new Mock<IVacationRequestSignalRService>();

		// When someone calls InitializeAsync(), hand them back a Task that’s already finished. Without it Moq would return null by default and your await would throw.
		mockSignalRService
			.Setup(s => s.InitializeAsync())
			.Returns(Task.CompletedTask);

		// Likewise for SendVacationRequestAsync():
		mockSignalRService
			.Setup(s => s.SendVacationRequestAsync())
			.Returns(Task.CompletedTask);

		// Register mocked services into the test's service collection
		Services.AddSingleton(mockDbFactory.Object);
		Services.AddSingleton(mockHubContext.Object);
		Services.AddSingleton(mockSignalRService.Object);

		// Setup fake authentication so the component thinks a user is logged in
		var authContext = this.AddTestAuthorization();
		authContext.SetAuthorized("testuser");
		authContext.SetClaims(new Claim(ClaimTypes.NameIdentifier, "test-user-id")); // Needed by the component to assign UserId
	}

	[TestMethod]
	public void SubmitForm_WithValidData_ShouldRedirectToList()
	{
		// Arrange

		// Mock the EF DbContext behavior
		var mockDbContext = new Mock<ApplicationDbContext>();
		mockDbContext.Setup(x => x.VacationRequests.Add(It.IsAny<VacationRequest>()));
		mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
					 .ReturnsAsync(1); // Simulate success

		// Ensure the component uses the mocked DbContext
		mockDbFactory.Setup(x => x.CreateDbContext())
					 .Returns(mockDbContext.Object);

		// Set up the fake navigation manager
		var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;

		// Act

		// Render the Create page component
		var component = RenderComponent<Create>();

		// Simulate user filling out form fields
		component.Find("#country").Change("Italy");
		component.Find("#startdate").Change(DateTime.Today);
		component.Find("#enddate").Change(DateTime.Today.AddDays(7));
		component.Find("#childrenamount").Change(2);
		component.Find("#adultsamount").Change(2);
		component.Find("#roomsamount").Change(1);
		component.Find("#hotelrequest").Change("Ocean View");
		component.Find("#flightrequest").Change("Morning flight");
		component.Find("#misc").Change("Vegetarian meals");

		// Submit the form
		component.Find("form").Submit();

		// Assert

		// We expect a redirect to the vacation request index page
		component.WaitForAssertion(() => Assert.AreEqual("http://localhost/vacationrequests", navMan!.Uri));
	}

	[TestMethod]
	public void SubmitForm_WithInvalidData_ShouldShowValidationErrors()
	{
		// Arrange and Act

		// Render the component
		var component = RenderComponent<Create>();

		// Enter invalid values
		component.Find("#childrenamount").Change(-1); // Invalid
		component.Find("#adultsamount").Change(0);
		component.Find("#roomsamount").Change(0);

		// Submit the form
		component.Find("form").Submit();

		// Assert

		// We expect validation error messages to appear
		component.WaitForAssertion(() =>
			Assert.IsTrue(component.Markup.Contains("text-danger"))
		);
	}

	[TestMethod]
	public void SubmitForm_ShouldCallSignalRMethods()
	{
		// Arrange

		// Mock the DbContext 
		var mockDbContext = new Mock<ApplicationDbContext>();
		mockDbContext.Setup(x => x.VacationRequests.Add(It.IsAny<VacationRequest>()));
		mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
		mockDbFactory.Setup(x => x.CreateDbContext()).Returns(mockDbContext.Object);


		// Render the component
		var component = RenderComponent<Create>();

		// Act

		// Simulate filling out form 
		component.Find("#country").Change("Italy");
		component.Find("#startdate").Change(DateTime.Today);
		component.Find("#enddate").Change(DateTime.Today.AddDays(7));
		component.Find("#childrenamount").Change(2);
		component.Find("#adultsamount").Change(2);
		component.Find("#roomsamount").Change(1);
		component.Find("#hotelrequest").Change("Ocean View");
		component.Find("#flightrequest").Change("Morning flight");
		component.Find("#misc").Change("Vegetarian meals");

		// Submit the form
		component.Find("form").Submit();

		// Assert

		// Confirm the SignalR method was called once (meaning the notification was sent)
		component.WaitForAssertion(() =>
			mockSignalRService.Verify(s => s.SendVacationRequestAsync(), Times.Once)
		);
	}
}
