using GotorzProjectMain.InputModels.VacationRequestInputModels;
using GotorzProjectMain.Models;
using System.ComponentModel.DataAnnotations;

namespace TestProject;

[TestClass]
public class VacationRequestModelValidationTests
{
	// Helper method that checks if the input model satisfies its data annotations (like [Required], [Range], etc.)
	// Returns a list of validation errors (if any)
	private IList<ValidationResult> Validate(object model)
	{
		var context = new ValidationContext(model);
		var results = new List<ValidationResult>();

		// Validates all properties of the object based on its attributes
		// Any broken rule will result in a ValidationResult being added to 'results'
		Validator.TryValidateObject(model, context, results, validateAllProperties: true);
		return results;
	}

	[TestMethod]
	public void ChildrenAmount_WhenNegative_ReturnsValidationError()
	{
		// Arrange: Create input with a negative ChildrenAmount
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

		// Act: Validate the model
		var results = Validate(request);

		// Assert: Look for a validation error specifically related to the ChildrenAmount property
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.ChildrenAmount))),
					  "ChildrenAmount should fail validation when negative");
	}

	[TestMethod]
	public void AdultsAmount_WhenNegative_ReturnsValidationError()
	{
		// Arrange: Create input with a negative AdultsAmount
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

		// Act: Validate the model
		var results = Validate(request);

		// Assert: Check for a validation error on the AdultsAmount property
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.AdultsAmount))),
					  "AdultsAmount should fail validation when negative");
	}

	[TestMethod]
	public void RoomsAmount_WhenNegative_ReturnsValidationError()
	{
		// Arrange: Create input with a negative RoomsAmount
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

		// Act: Validate the model
		var results = Validate(request);

		// Assert: Check for a validation error on the RoomsAmount property
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.RoomsAmount))),
					  "RoomsAmount should fail validation when negative");
	}

	[TestMethod]
	public void AllAmounts_WhenNonNegative_ReturnsNoValidationErrors()
	{
		// Arrange: Create input with valid (non-negative) values
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

		// Act: Validate the model
		var results = Validate(request);

		// Assert: No validation errors should occur
		Assert.AreEqual(0, results.Count, "No validation errors expected when all counts are non-negative");
	}
}