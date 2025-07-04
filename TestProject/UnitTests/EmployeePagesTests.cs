﻿using AutoMapper;
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
        mockUserService = new Mock<IExtendedUserService>();
        mockCurrentUserService = new Mock<ICurrentUserService>();
        mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null
        );
        
        var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        Services.AddSingleton(mockContext.Object);
        
        var mockMapper = new Mock<IMapper>();
        Services.AddSingleton(mockMapper.Object);
        
        var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        mockWebHostEnvironment.Setup(e => e.WebRootPath).Returns(Path.GetTempPath()); // You can set a temp folder
        Services.AddSingleton(mockWebHostEnvironment.Object);

        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(mockUserManager.Object);
        Services.AddSingleton(mockCurrentUserService.Object);

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

        navMan.NavigateTo("http://localhost:nnnn/employees/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<DetailsEmployee>();

        // Assert
        Assert.AreEqual("http://localhost:nnnn/notfound", navMan.Uri);
    }

    // --------- EditEmployee Tests ---------
    [TestMethod]
    public void EditEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

        navMan.NavigateTo("http://localhost:nnnn/employees/edit?id=nonexistent-id");

        // Act
        var cut = RenderComponent<EditEmployee>();

        // Assert
        Assert.AreEqual("http://localhost:nnnn/employees", navMan.Uri);
    }

    // --------- RemoveEmployee Tests ---------
    [TestMethod]
    public void RemoveEmployee_OnInitializedAsync_ShouldRedirectToNotFound_WhenEmployeeIsNull()
    {
        // Arrange
        mockUserService
            .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

        navMan.NavigateTo("http://localhost:nnnn/employees/details?id=nonexistent-id");

        // Act
        var cut = RenderComponent<RemoveEmployee>(); // ⬅️ RemoveEmployee

        // Assert
        Assert.AreEqual("http://localhost:nnnn/notfound", navMan.Uri);
    }

}
