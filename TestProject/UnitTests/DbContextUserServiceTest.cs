using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject.UnitTests
{
    // Wrote this test class to test the UserService class' behavior with a simulated database context, but since ApplicationDbContext cant me simulated i would have to make an in memory database to do so.
    // Mainly written with chatgpt

    //[TestClass]
    //public class UserServiceTests
    //{
    //    private Mock<IDbContextFactory<ApplicationDbContext>> _mockDbFactory;
    //    private Mock<ApplicationDbContext> _mockDbContext;
    //    private Mock<UserManager<ApplicationUser>> _mockUserManager;
    //    private IUserService _userService;

    //    private Mock<DbSet<Customer>> _mockCustomerDbSet;
    //    private Mock<DbSet<Employee>> _mockEmployeeDbSet;

    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        // 1. Create mock Customer and Employee lists
    //        var customers = new List<Customer>
    //        {
    //            new Customer { Id = "1", CustomUserName = "TestCustomer1", User = new ApplicationUser { FirstName = "Alice" } },
    //            new Customer { Id = "2", CustomUserName = "TestCustomer2", User = new ApplicationUser { FirstName = "Bob" } }
    //        }.AsQueryable();

    //        var employees = new List<Employee>
    //        {
    //            new Employee { Id = "1", Role = true, User = new ApplicationUser { FirstName = "Charlie" } },
    //            new Employee { Id = "2", Role = false, User = new ApplicationUser { FirstName = "Diana" } }
    //        }.AsQueryable();

    //        // 2. Mock DbSet<Customer>
    //        _mockCustomerDbSet = new Mock<DbSet<Customer>>();
    //        _mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
    //        _mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
    //        _mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
    //        _mockCustomerDbSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

    //        // 3. Mock DbSet<Employee>
    //        _mockEmployeeDbSet = new Mock<DbSet<Employee>>();
    //        _mockEmployeeDbSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employees.Provider);
    //        _mockEmployeeDbSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employees.Expression);
    //        _mockEmployeeDbSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employees.ElementType);
    //        _mockEmployeeDbSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employees.GetEnumerator());

    //        // 4. Mock ApplicationDbContext
    //        _mockDbContext = new Mock<ApplicationDbContext>();
    //        _mockDbContext.Setup(db => db.Customers).Returns(_mockCustomerDbSet.Object);
    //        _mockDbContext.Setup(db => db.Employees).Returns(_mockEmployeeDbSet.Object);

    //        // 5. Mock DbContextFactory
    //        _mockDbFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
    //        _mockDbFactory.Setup(factory => factory.CreateDbContext()).Returns(_mockDbContext.Object);

    //        // 6. Mock UserManager (minimum setup)
    //        var store = new Mock<IUserStore<ApplicationUser>>();
    //        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
    //            store.Object, null, null, null, null, null, null, null, null
    //        );

    //        // 7. Create the real UserService with mocked dependencies
    //        _userService = new UserService(_mockDbFactory.Object, _mockUserManager.Object);
    //    }

    //    // 🧪 Now your tests here (same as before)...

    //    [TestMethod]
    //    public async Task GetCustomersWithUsersAsync_ReturnsAllCustomers()
    //    {
    //        var customers = await _userService.GetCustomersWithUsersAsync();
    //        Assert.IsNotNull(customers);
    //        Assert.AreEqual(2, customers.Count);
    //    }

    //    [TestMethod]
    //    public async Task GetCustomerByIdAsync_ReturnsCustomer_WhenExists()
    //    {
    //        var customer = await _userService.GetCustomerByIdAsync("1");
    //        Assert.IsNotNull(customer);
    //        Assert.AreEqual("TestCustomer1", customer.CustomUserName);
    //    }

    //    [TestMethod]
    //    public async Task GetCustomerByIdAsync_ReturnsNull_WhenNotExists()
    //    {
    //        var customer = await _userService.GetCustomerByIdAsync("999");
    //        Assert.IsNull(customer);
    //    }

    //    [TestMethod]
    //    public async Task GetEmployeesWithUsersAsync_ReturnsAllEmployees()
    //    {
    //        var employees = await _userService.GetEmployeesWithUsersAsync();
    //        Assert.IsNotNull(employees);
    //        Assert.AreEqual(2, employees.Count);
    //    }

    //    [TestMethod]
    //    public async Task GetEmployeeByIdAsync_ReturnsEmployee_WhenExists()
    //    {
    //        var employee = await _userService.GetEmployeeByIdAsync("1");
    //        Assert.IsNotNull(employee);
    //        Assert.AreEqual("Charlie", employee.User.FirstName);
    //    }

    //    [TestMethod]
    //    public async Task GetEmployeeByIdAsync_ReturnsNull_WhenNotExists()
    //    {
    //        var employee = await _userService.GetEmployeeByIdAsync("999");
    //        Assert.IsNull(employee);
    //    }
    //}
}
