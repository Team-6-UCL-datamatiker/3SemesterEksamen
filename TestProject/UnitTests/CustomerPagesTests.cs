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
using GotorzProjectMain.Components.Pages.CustomerPages;

namespace TestProject.UnitTests;

[TestClass]
public class CustomerPagesTests : Bunit.TestContext
{
    private Mock<IExtendedUserService> mockUserService;
    private Mock<UserManager<ApplicationUser>> mockUserManager;
    private FakeNavigationManager navMan;

    [TestInitialize]
    public void Setup()
    {
		// Setup mocked services and dependencies needed by the components
		mockUserService = new Mock<IExtendedUserService>();

		// Creating a mock of UserManager with dummy dependencies
		mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null
        );

		// Setup fake DbContext
		var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        Services.AddSingleton(mockContext.Object);

		// Add a mocked AutoMapper service
		var mockMapper = new Mock<IMapper>();
        Services.AddSingleton(mockMapper.Object);

		// Mock the WebHostEnvironment for any file-related services
		var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        mockWebHostEnvironment.Setup(e => e.WebRootPath).Returns(Path.GetTempPath()); // You can set a temp folder
        Services.AddSingleton(mockWebHostEnvironment.Object);

		// Register the mocked services in the test's service provider
		Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockUserManager.Object);

		// Capture and store the fake NavigationManager to check redirection
		navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;
    }

    // --------- DetailsCustomer Tests ---------
    [TestMethod]
    public void DetailsCustomer_OnInitializedAsync_ShouldRedirectToNotFound_WhenCustomerIsNull()
    {
		// Arrange: Simulate that the customer was not found in the database
		mockUserService
			.Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Customer)null);

		// Set the starting URL (as if the user is navigating to details page)
		navMan.NavigateTo("http://localhost/customers/details?id=nonexistent-id");

		// Act: Render the DetailsCustomer component
		var cut = RenderComponent<DetailsCustomer>();

		// Assert: If no customer found, we expect a redirect to the "not found" page
		Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }

    // --------- EditCustomer Tests ---------
    [TestMethod]
    public void EditCustomer_OnInitializedAsync_ShouldRedirectToNotFound_WhenCustomerIsNull()
    {
		// Arrange: Simulate no matching customer found
		mockUserService
			.Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Customer)null);

		// Navigate to edit page
		navMan.NavigateTo("http://localhost/customers/details?id=nonexistent-id");

		// Act: Render the EditCustomer component
		var cut = RenderComponent<EditCustomer>();

		// Assert: Should redirect back to main customers list
        Assert.AreEqual("http://localhost/localhost/customers", navMan.Uri);
    }

    // --------- RemoveCustomer Tests ---------
    [TestMethod]
    public void RemoveCustomer_OnInitializedAsync_ShouldRedirectToNotFound_WhenCustomerIsNull()
    {
		// Arrange: Simulate no matching customer found
		mockUserService
			.Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Customer)null);

		// Navigate to remove page
		navMan.NavigateTo("http://localhost/customers/details?id=nonexistent-id");

		// Act: Render the RemoveCustomer component
		var cut = RenderComponent<RemoveCustomer>();

		// Assert: Should redirect to "not found" page
		Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }
}
