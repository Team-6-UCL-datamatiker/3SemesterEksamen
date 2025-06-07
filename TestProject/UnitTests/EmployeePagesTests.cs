using AutoMapper;
using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services;
using Microsoft.AspNetCore.Components;
using Moq;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GotorzProjectMain.Components.Pages.EmployeePages;

namespace TestProject.UnitTests;

[TestClass]
public class EmployeePagesTests : Bunit.TestContext
{
    private Mock<IExtendedUserService> mockUserService;
    private Mock<UserManager<ApplicationUser>> mockUserManager;
    private Mock<ICurrentUserService> mockCurrentUserService;
    private FakeNavigationManager navMan;

    [TestInitialize]
    public void Setup()
    {
		// Create mocks for required services
		mockUserService = new Mock<IExtendedUserService>();
        mockCurrentUserService = new Mock<ICurrentUserService>();

		// Create a mock UserManager with dummy parameters
		mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null
        );

		// Provide a mocked DB context
		var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        Services.AddSingleton(mockContext.Object);

		// Provide a mocked AutoMapper
		var mockMapper = new Mock<IMapper>();
        Services.AddSingleton(mockMapper.Object);

		// Provide a mocked WebHostEnvironment for components that use file paths
		var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        mockWebHostEnvironment.Setup(e => e.WebRootPath).Returns(Path.GetTempPath()); 
        Services.AddSingleton(mockWebHostEnvironment.Object);

		// Register mocks in the DI container
		Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockUserManager.Object);
        Services.AddSingleton(mockCurrentUserService.Object);

		// Get the fake navigation manager for tracking redirects
		navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;
    }

    // --------- DetailsEmployee Tests ---------
    [TestMethod]
    public void DetailsEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
		// Arrange: Return null to simulate employee not found
		mockUserService
			.Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

		// Navigate to employee details page
		navMan.NavigateTo("/employees/details?id=nonexistent-id");

		// Act: Render the component
		var cut = RenderComponent<DetailsEmployee>();

		// Assert: Should redirect to notfound page
		Assert.AreEqual("/notfound", navMan.Uri);
    }

    // --------- EditEmployee Tests ---------
    [TestMethod]
    public void EditEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
		// Arrange: Return null to simulate employee not found
		mockUserService
			.Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

		// Navigate to edit employee page
		navMan.NavigateTo("/employees/edit?id=nonexistent-id");

		// Act: Render the component
		var cut = RenderComponent<EditEmployee>();

		// Assert: Should redirect to employee list
		Assert.AreEqual("/employees", navMan.Uri);
    }

    // --------- RemoveEmployee Tests ---------
    [TestMethod]
    public void RemoveEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
		// Arrange: Return null to simulate employee not found
		mockUserService
			.Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);
		
        // Navigate to remove employee page
		navMan.NavigateTo("/employees/details?id=nonexistent-id");

		// Act: Render the component
		var cut = RenderComponent<RemoveEmployee>(); // ⬅️ RemoveEmployee

		// Assert: Should redirect to notfound page
		Assert.AreEqual("/notfound", navMan.Uri);
    }

}
