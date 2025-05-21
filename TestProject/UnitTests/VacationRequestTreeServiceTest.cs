using GotorzProjectMain.Models;
using GotorzProjectMain.Services;
using Moq;

namespace TestProject.UnitTests
{
    [TestClass]
    public sealed class MockVacationRequestTreeServiceTest
    {
        private Mock<IVacationRequestTreeService> mockTreeService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockTreeService = new Mock<IVacationRequestTreeService>();

            var testRequest = new VacationRequest
            {
                VacationRequestId = 1,
                DepartureCity = "Copenhagen",
                ArrivalCity = "Paris",
                Offers = new List<VacationOffer>
                {
                    new VacationOffer
                    {
                        VacationOfferId = 1,
                        HotelBooking = new HotelBooking
                        {
                            HotelBookingId = 1,
                            HotelName = "Test Hotel"
                        },
                        FlightBooking = new FlightBooking
                        {
                            FlightBookingId = 1,
                            FlightRoutes = new List<FlightRoute>
                            {
                                new FlightRoute
                                {
                                    RouteId = 1,
                                    Legs = new List<Flight>
                                    {
                                        new Flight
                                        {
                                            FlightId = 1,
                                            DepartureAirportCode = "CPH",
                                            ArrivalAirportCode = "CDG"
                                        }
                                    },
                                    Layovers = new List<Layover>
                                    {
                                        new Layover
                                        {
                                            LayoverId = 1,
                                            Name = "Amsterdam",
                                            Duration = 90
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var testFlightBooking = testRequest.Offers.First().FlightBooking;

            // Setup: known ID returns data
            mockTreeService
                .Setup(x => x.LoadVacationRequestTreeByIdAsync(1))
                .ReturnsAsync(testRequest);

            // Setup: unknown ID throws
            mockTreeService
                .Setup(x => x.LoadVacationRequestTreeByIdAsync(It.Is<int>(id => id != 1)))
                .ThrowsAsync(new InvalidOperationException("VacationRequest not found"));



        }

        [TestMethod]
        public void PositiveLoadVacationRequestTreeByIdAsyncTest()
        {
            // Arrange
            int requestId = 1;

            // Act
            var result = mockTreeService.Object.LoadVacationRequestTreeByIdAsync(requestId).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Copenhagen", result.DepartureCity);
            Assert.AreEqual("Paris", result.ArrivalCity);
            Assert.AreEqual(1, result.Offers.Count);
            Assert.AreEqual("Test Hotel", result.Offers.First().HotelBooking.HotelName);
            Assert.AreEqual("CPH", result.Offers.First().FlightBooking.FlightRoutes.First().Legs.First().DepartureAirportCode);
        }

        [TestMethod]
        public void NegativeLoadVacationRequestTreeByIdAsyncTest()
        {
            // Arrange
            int invalidRequestId = 999;

            // Act & Assert
            Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await mockTreeService.Object.LoadVacationRequestTreeByIdAsync(invalidRequestId);
            });
        }
        

    }
}
