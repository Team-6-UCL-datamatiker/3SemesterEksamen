using GotorzProjectMain.InputModels.VacationRequestInputModels;
using GotorzProjectMain.Models;
using System.ComponentModel.DataAnnotations;

namespace TestProject;

[TestClass]
public class VacationRequestModelValidationTests
{
	// Helper method to validate the request
	private IList<ValidationResult> Validate(object model)
	{
		var context = new ValidationContext(model);
		var results = new List<ValidationResult>();

		// checks all properties of the request. If any are invalid, it adds a ValidationResult to the results list
		Validator.TryValidateObject(model, context, results, validateAllProperties: true);
		return results;
	}

	[TestMethod]
	public void ChildrenAmount_WhenNegative_ReturnsValidationError()
	{
		// Arrange
		var request = new CreateVacationRequestInputModel
		{
			DepartureCity = "Copenhagen",
			ArrivalCity = "Paris",
			DepartureCountry = "Denmark",
			ArrivalCountry = "France",
			ChildrenAmount = -1,
			AdultsAmount = 1,
			RoomsAmount = 1,
			StartDate = DateTime.Today,
			EndDate = DateTime.Today.AddDays(7)
		};

		// Act
		var results = Validate(request);

		// Assert
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.ChildrenAmount))),
					  "ChildrenAmount should fail validation when negative");
	}

	[TestMethod]
	public void AdultsAmount_WhenNegative_ReturnsValidationError()
	{
		// Arrange
		var request = new CreateVacationRequestInputModel
		{
			DepartureCity = "Copenhagen",
			ArrivalCity = "Paris",
			DepartureCountry = "Denmark",
			ArrivalCountry = "France",
			ChildrenAmount = 0,
			AdultsAmount = -5,
			RoomsAmount = 1,
			StartDate = DateTime.Today,
			EndDate = DateTime.Today.AddDays(7)
		};

		// Act
		var results = Validate(request);

		// Assert
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.AdultsAmount))),
					  "AdultsAmount should fail validation when negative");
	}

	[TestMethod]
	public void RoomsAmount_WhenNegative_ReturnsValidationError()
	{
		// Arrange
		var request = new CreateVacationRequestInputModel
		{
			DepartureCity = "Copenhagen",
			ArrivalCity = "Paris",
			DepartureCountry = "Denmark",
			ArrivalCountry = "France",
			ChildrenAmount = 0,
			AdultsAmount = 1,
			RoomsAmount = -2,
			StartDate = DateTime.Today,
			EndDate = DateTime.Today.AddDays(7)
		};

		// Act
		var results = Validate(request);

		// Assert
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.RoomsAmount))),
					  "RoomsAmount should fail validation when negative");
	}

	[TestMethod]
	public void AllAmounts_WhenNonNegative_ReturnsNoValidationErrors()
	{
		// Arrange
		var request = new CreateVacationRequestInputModel
		{
			DepartureCity = "Copenhagen",
			ArrivalCity = "Paris",
			DepartureCountry = "Denmark",
			ArrivalCountry = "France",
			ChildrenAmount = 1,
			AdultsAmount = 2,
			RoomsAmount = 3,
			StartDate = DateTime.Today,
			EndDate = DateTime.Today.AddDays(7)
		};

		// Act
		var results = Validate(request);

		// Assert
		Assert.AreEqual(0, results.Count, "No validation errors expected when all counts are non-negative");
	}
}