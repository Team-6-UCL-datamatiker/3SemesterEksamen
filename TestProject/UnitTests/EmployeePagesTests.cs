using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services;
using Microsoft.AspNetCore.Components;
using Moq;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Bunit.TestDoubles;
using GotorzProjectMain.Components.Account.Pages.CRUD.EmployeePages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TestProject.UnitTests;

[TestClass]
public class EmployeePagesTests : Bunit.TestContext
{
    private Mock<IExtendedUserService> mockUserService;
    private Mock<IDbContextFactory<ApplicationDbContext>> mockDbFactory;
    private Mock<UserManager<ApplicationUser>> mockUserManager;
    private FakeNavigationManager navMan;

    [TestInitialize]
    public void Setup()
    {
        mockUserService = new Mock<IExtendedUserService>();
        mockDbFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
        mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null
        );

        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockDbFactory.Object);
        Services.AddSingleton(mockUserManager.Object);

        navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;
    }

    // --------- DetailsEmployee Tests ---------
    [TestMethod]
    public void DetailsEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

        navMan.NavigateTo("http://localhost/employees/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<DetailsEmployee>();

        // Assert
        Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }

    // --------- EditEmployee Tests ---------
    [TestMethod]
    public void EditEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

        navMan.NavigateTo("http://localhost/employees/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<EditEmployee>();

        // Assert
        Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }

    [TestMethod]
    public void EditEmployee_EmployeeExists_ShouldReturnTrue_WhenEmployeeExists()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public void EditEmployee_EmployeeExists_ShouldReturnFalse_WhenEmployeeDoesNotExist()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public async Task EditEmployee_UpdateEmployee_ShouldRedirectToNotFound_WhenConcurrencyExceptionAndEmployeeMissing()
    {
        // Arrange

        // Act

        // Assert
    }

    // --------- RegisterEmployee Tests ---------
    [TestMethod]
    public async Task RegisterEmployee_RegisterUser_ShouldSetIdentityErrors_WhenCreateUserFails()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public async Task RegisterEmployee_RegisterUser_ShouldSetIdentityErrors_WhenAddToRoleFails()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public void RegisterEmployee_Message_ShouldFormatErrorMessagesCorrectly()
    {
        // Arrange

        // Act

        // Assert
    }

    // --------- RemoveEmployee Tests ---------
    [TestMethod]
    public void RemoveEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

        navMan.NavigateTo("http://localhost/employees/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<RemoveEmployee>(); // ⬅️ RemoveEmployee

        // Assert
        Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }

    [TestMethod]
    public async Task RemoveEmployee_DeleteEmployee_ShouldRedirectToEmployees_WhenDeleted()
    {
        // Arrange

        // Act

        // Assert
    }
}
