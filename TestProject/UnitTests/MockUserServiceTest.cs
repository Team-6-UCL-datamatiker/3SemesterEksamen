using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using GotorzProjectMain.Services;
using Moq;

namespace TestProject.UnitTests
{
    [TestClass]
    public sealed class MockUserServiceTest
    {
        private Mock<IExtendedUserService> mockUserService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUserService = new Mock<IExtendedUserService>();

            List<Customer> mockCustomers = new List<Customer>
            {
                new Customer
                {
                    Id = "1",
                    CustomUserName = "TestUser1",
                    User = new ApplicationUser { UserName = "TestUser1" }
                },
                new Customer
                {
                    Id = "2",
                    CustomUserName = "TestUser2",
                    User = new ApplicationUser { UserName = "TestUser2" }
                }
            };
            List<Employee> mockEmployees = new List<Employee>
            {
                new Employee
                {
                    Id = "1",
                    ProfilePicture = new Uri("https://example.com/profile1.jpg").ToString(),
                    IsAdmin = true,
                    User = new ApplicationUser { UserName = "TestUser1" }
                },
                new Employee
                {
                    Id = "2",
                    ProfilePicture = new Uri("https://example.com/profile2.jpg").ToString(),
                    IsAdmin = false,
                    User = new ApplicationUser { UserName = "TestUser2" }
                }
            };


            mockUserService
                .Setup(x => x.GetCustomersWithUsersAsync())
                .ReturnsAsync(mockCustomers);
            mockUserService
                .Setup(x => x.GetEmployeesWithUsersAsync())
                .ReturnsAsync(mockEmployees);
            mockUserService
                .Setup(x => x.GetCustomerByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => mockCustomers.FirstOrDefault(c => c.Id == id));
            mockUserService
                .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => mockEmployees.FirstOrDefault(e => e.Id == id));
        }


        [TestMethod]
        public void GetCustomersWithUsersAsync_WhenCustomersExist_ReturnsCorrectCount()
        {
            // Arrange
            int expectedCustomers = 2;
            // Act
            var result = mockUserService.Object.GetCustomersWithUsersAsync().Result;
            // Assert
            Assert.AreEqual(expectedCustomers, result.Count);
        }
        [TestMethod]
        public void GetCustomersWithUsersAsync_WhenExpectedCountIsIncorrect_ReturnsMismatchedCount()
        {
            // Arrange
            int expectedCustomers = 99;
            // Act
            var result = mockUserService.Object.GetCustomersWithUsersAsync().Result;
            // Assert
            Assert.AreNotEqual(expectedCustomers, result.Count);
        }
        [TestMethod]
        public void GetEmployeesWithUsersAsync_WhenEmployeesExist_ReturnsCorrectCount()
        {
            // Arrange
            int expectedEmployees = 2;
            // Act
            var result = mockUserService.Object.GetEmployeesWithUsersAsync().Result;
            // Assert
            Assert.AreEqual(expectedEmployees, result.Count);
        }
        [TestMethod]
        public void GetEmployeesWithUsersAsync_WhenExpectedCountIsIncorrect_ReturnsMismatchedCount()
        {
            // Arrange
            int expectedEmployees = 99;
            // Act
            var result = mockUserService.Object.GetEmployeesWithUsersAsync().Result;
            // Assert
            Assert.AreNotEqual(expectedEmployees, result.Count);
        }
        [TestMethod]
        public void GetCustomerByIdAsync_WhenCustomerIdExists_ReturnsCustomer()
        {
            // Arrange
            Customer expectedCustomer = new Customer
            {
                Id = "1",
                CustomUserName = "TestUser1",
                User = new ApplicationUser { UserName = "TestUser1" }
            };
            // Act
            var result = mockUserService.Object.GetCustomerByIdAsync(expectedCustomer.Id).Result;
            // Assert
            Assert.AreEqual(expectedCustomer.Id, result?.Id);
        }
        [TestMethod]
        public void GetCustomerByIdAsync_WhenCustomerIdDoesNotExist_ReturnsNull()
        {
            // Arrange
            Customer expectedCustomer = new Customer
            {
                Id = "99",
                CustomUserName = "TestUser99",
                User = new ApplicationUser { UserName = "TestUser99" }
            };
            // Act
            var result = mockUserService.Object.GetCustomerByIdAsync(expectedCustomer.Id).Result;
            // Assert
            Assert.AreNotEqual(expectedCustomer.Id, result?.Id);
        }
        [TestMethod]
        public void GetEmployeeByIdAsync_WhenEmployeeIdExists_ReturnsEmployee()
        {
            // Arrange
            Employee expectedEmployee = new Employee
            {
                Id = "1",
                ProfilePicture = new Uri("https://example.com/profile1.jpg").ToString(),
                IsAdmin = true,
                User = new ApplicationUser { UserName = "TestUser1" }
            };

            // Act
            var result = mockUserService.Object.GetEmployeeByIdAsync(expectedEmployee.Id).Result;

            // Assert
            Assert.AreEqual(expectedEmployee.Id, result?.Id);
        }
        [TestMethod]
        public void GetEmployeeByIdAsync_WhenEmployeeIdDoesNotExist_ReturnsNull()
        {
            // Arrange
            Employee expectedEmployee = new Employee
            {
                Id = "99",
                ProfilePicture = new Uri("https://example.com/profile1.jpg").ToString(),
                IsAdmin = true,
                User = new ApplicationUser { UserName = "TestUser99" }
            };

            // Act
            var result = mockUserService.Object.GetEmployeeByIdAsync(expectedEmployee.Id).Result;

            // Assert
            Assert.AreNotEqual(expectedEmployee.Id, result?.Id);
        }
    }
}
