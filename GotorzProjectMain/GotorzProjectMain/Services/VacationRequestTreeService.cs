using GotorzProjectMain.Data;
using GotorzProjectMain.Models;
using Microsoft.EntityFrameworkCore;

namespace GotorzProjectMain.Services
{
	public class VacationRequestTreeService : IVacationRequestTreeService
	{
		private readonly ApplicationDbContext Context;

		public VacationRequestTreeService(ApplicationDbContext context)
		{
			Context = context;
		}

		// Loads a complete vacation request with all related data (offers, bookings, routes, flights)
		public async Task<VacationRequest> LoadVacationRequestTreeByIdAsync(int vacationRequestId)
		{
			// Each path (FlightRoutes → Legs, FlightRoutes → Layovers, and HotelBooking) must be included separately.
			// EF Core requires full paths to all nested data; it does not automatically follow sibling relationships.

			VacationRequest? vacationRequest = await Context.VacationRequests
				// Outbound flight
				.Include(vr => vr.Offers)
					.ThenInclude(o => o.FlightBooking)
						.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Legs)
				// Return flight
				.Include(vr => vr.Offers)
					.ThenInclude(o => o.FlightBooking)
						.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Layovers)
				// Hotel booking
				.Include(vr => vr.Offers)
					.ThenInclude(o => o.HotelBooking)
				.FirstOrDefaultAsync(vr => vr.VacationRequestId == vacationRequestId);

			if (vacationRequest == null)
			{
				throw new InvalidOperationException($"VacationRequest with ID {vacationRequestId} not found.");
			}

			return vacationRequest;
		}

		// Reloads an existing vacation request from the database to get updated related data
		public async Task<VacationRequest> ReloadVacationRequestTreeAsync(VacationRequest vacationRequest)
		{
			var refreshedRequest = await Context.VacationRequests
				// Outbound flight
				.Include(vr => vr.Offers)
					.ThenInclude(o => o.FlightBooking)
						.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Legs)
				// Return flight
				.Include(vr => vr.Offers)
					.ThenInclude(o => o.FlightBooking)
						.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Layovers)
				// Hotel booking
				.Include(vr => vr.Offers)
					.ThenInclude(o => o.HotelBooking)
				.FirstOrDefaultAsync(vr => vr.VacationRequestId == vacationRequest.VacationRequestId);

			if (refreshedRequest == null)
			{
				throw new InvalidOperationException($"VacationRequest with ID {vacationRequest.VacationRequestId} not found.");
			}

			return refreshedRequest;
		}

		// Loads a single vacation offer with its bookings and flight data
		public async Task<VacationOffer> LoadVacationOfferTreeAsync(VacationOffer vacationOffer)
		{
			var refreshedOffer = await Context.VacationOffers
				// outbound flight
				.Include(o => o.FlightBooking)
				.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Legs)
				// return flight
				.Include(o => o.FlightBooking)
						.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Layovers)
				// hotel booking
				.Include(o => o.HotelBooking)
				.FirstOrDefaultAsync(o => o.VacationOfferId == vacationOffer.VacationOfferId);

			if (refreshedOffer == null)
			{
				throw new InvalidOperationException($"VacationOffer with ID {vacationOffer.VacationOfferId} not found.");
			}

			return refreshedOffer;
		}

		// Loads all offers related to a specific vacation request, including bookings and flight info
		public async Task<List<VacationOffer>> LoadVacationOffersTreeByRequestIdAsync(VacationRequest vacationRequest)
		{
			List<VacationOffer> vacationOffers = await Context.VacationOffers
				// Outbound flight
				.Include(o => o.FlightBooking)
				.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Legs)
				// Return flight
				.Include(o => o.FlightBooking)
						.ThenInclude(fb => fb.FlightRoutes)
							.ThenInclude(fr => fr.Layovers)
				// Hotel booking
				.Include(o => o.HotelBooking)
				.Where(o => o.VacationRequestId == vacationRequest.VacationRequestId)
				.ToListAsync();
			return vacationOffers;

		}
	}
}
