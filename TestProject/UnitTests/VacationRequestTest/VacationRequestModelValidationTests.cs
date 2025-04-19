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
	public void ChildrenAmount_Negative_IsInvalid()
	{
		// Arrange
		var request = new VacationRequest { ChildrenAmount = -1, AdultsAmount = 0, RoomsAmount = 0 };
		
		// Act
		var results = Validate(request);

		// Assert
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.ChildrenAmount))),
					  "ChildrenAmount skal fejle validering ved -1"); // This passes if the ChildrenAmount property failed validation
	}

	[TestMethod]
	public void AdultsAmount_Negative_IsInvalid()
	{
		// Arrange
		var request = new VacationRequest { ChildrenAmount = 0, AdultsAmount = -5, RoomsAmount = 0 };

		// Act
		var results = Validate(request);

		// Assert
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.AdultsAmount))),
					  "AdultsAmount skal fejle validering ved -5"); // This passes if the AdultsAmount property failed validation
	}

	[TestMethod]
	public void RoomsAmount_Negative_IsInvalid()
	{
		// Arrange
		var request = new VacationRequest { ChildrenAmount = 0, AdultsAmount = 0, RoomsAmount = -2 };

		// Act
		var results = Validate(request);

		// Assert
		Assert.IsTrue(results.Any(r => r.MemberNames.Contains(nameof(request.RoomsAmount))),
					  "RoomsAmount skal fejle validering ved -2"); // This passes if the RoomsAmount property failed validation
	}

	[TestMethod]
	public void AllAmounts_NonNegative_IsValid()
	{
		// Arrange
		var vr = new VacationRequest { ChildrenAmount = 1, AdultsAmount = 2, RoomsAmount = 3 };

		// Act
		var results = Validate(vr);

		// Assert
		Assert.AreEqual(0, results.Count, "Ingen valideringsfejl forventes når alle ≥ 0");
	}
}
