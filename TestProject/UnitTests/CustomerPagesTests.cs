using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services;
using Microsoft.AspNetCore.Components;
using Moq;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Bunit.TestDoubles;
using GotorzProjectMain.Components.Account.Pages.CRUD.CustomerPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TestProject.UnitTests;

[TestClass]
public class CustomerPagesTests : Bunit.TestContext
{
    private Mock<IUserService> mockUserService;
    private Mock<IDbContextFactory<ApplicationDbContext>> mockDbFactory;
    private Mock<UserManager<ApplicationUser>> mockUserManager;
    private FakeNavigationManager navMan;

    [TestInitialize]
    public void Setup()
    {
        mockUserService = new Mock<IUserService>();
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

    // --------- DetailsCustomer Tests ---------
    [TestMethod]
    public void DetailsCustomer_OnInitializedAsync_ShouldRedirectToNotFound_WhenCustomerIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Customer)null);

        navMan.NavigateTo("http://localhost/customers/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<DetailsCustomer>();

        // Assert
        Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }

    // --------- EditCustomer Tests ---------
    [TestMethod]
    public void EditCustomer_OnInitializedAsync_ShouldRedirectToNotFound_WhenCustomerIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Customer)null);

        navMan.NavigateTo("http://localhost/customers/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<EditCustomer>();

        // Assert
        Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }

    [TestMethod]
    public void EditCustomer_CustomerExists_ShouldReturnTrue_WhenCustomerExists()
    {
        //// Arrange
        //mockUserService
        //    .Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
        //    .ReturnsAsync(new Customer { Id = "existing-id" });
        //// Act
        //var cut = RenderComponent<EditCustomer>();
        //// Assert
        //Assert.IsTrue(cut.Instance.CustomerExists("existing-id"));
    }

    [TestMethod]
    public void EditCustomer_CustomerExists_ShouldReturnFalse_WhenCustomerDoesNotExist()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public async Task EditCustomer_UpdateCustomer_ShouldRedirectToNotFound_WhenConcurrencyExceptionAndCustomerMissing()
    {
        // Arrange

        // Act

        // Assert
    }

    // --------- RegisterCustomer Tests ---------
    [TestMethod]
    public async Task RegisterCustomer_RegisterUser_ShouldSetIdentityErrors_WhenCreateUserFails()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public async Task RegisterCustomer_RegisterUser_ShouldSetIdentityErrors_WhenAddToRoleFails()
    {
        // Arrange

        // Act

        // Assert
    }

    [TestMethod]
    public void RegisterCustomer_Message_ShouldFormatErrorMessagesCorrectly()
    {
        // Arrange

        // Act

        // Assert
    }

    // --------- RemoveCustomer Tests ---------
    [TestMethod]
    public void RemoveCustomer_OnInitializedAsync_ShouldRedirectToNotFound_WhenCustomerIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Customer)null);

        navMan.NavigateTo("http://localhost/customers/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<RemoveCustomer>();

        // Assert
        Assert.AreEqual("http://localhost/notfound", navMan.Uri);
    }

    [TestMethod]
    public async Task RemoveCustomer_DeleteCustomer_ShouldRedirectToCustomers_WhenDeleted()
    {
        // Arrange

        // Act

        // Assert
    }
}
