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

        public async Task<VacationRequest> LoadVacationRequestTreeByIdAsync(int vacationRequestId)
        {
            VacationRequest? vacationRequest = await Context.VacationRequests
                .Include(vr => vr.Offers)
                    .ThenInclude(o => o.FlightBooking)
                        .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Legs)
                .Include(vr => vr.Offers)
                    .ThenInclude(o => o.FlightBooking)
                        .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Layovers)
                .Include(vr => vr.Offers)
                    .ThenInclude(o => o.HotelBooking)
                .FirstOrDefaultAsync(vr => vr.VacationRequestId == vacationRequestId);

            if (vacationRequest == null)
            {
                throw new InvalidOperationException($"VacationRequest with ID {vacationRequestId} not found.");
            }

            return vacationRequest;
        }

        public async Task<VacationRequest> ReloadVacationRequestTreeAsync(VacationRequest vacationRequest)
        {
            var refreshedRequest = await Context.VacationRequests
                .Include(vr => vr.Offers)
                    .ThenInclude(o => o.FlightBooking)
                        .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Legs)
                .Include(vr => vr.Offers)
                    .ThenInclude(o => o.FlightBooking)
                        .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Layovers)
                .Include(vr => vr.Offers)
                    .ThenInclude(o => o.HotelBooking)
                .FirstOrDefaultAsync(vr => vr.VacationRequestId == vacationRequest.VacationRequestId);

            if (refreshedRequest == null)
            {
                throw new InvalidOperationException($"VacationRequest with ID {vacationRequest.VacationRequestId} not found.");
            }

            return refreshedRequest;
        }

        public async Task<VacationOffer> LoadVacationOfferTreeAsync(VacationOffer vacationOffer)
        {
            var refreshedOffer = await Context.VacationOffers
                .Include(o => o.FlightBooking)
                .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Legs)
                .Include(o => o.FlightBooking)
                        .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Layovers)
                .Include(o => o.HotelBooking)
                .FirstOrDefaultAsync(o => o.VacationOfferId == vacationOffer.VacationOfferId);
            
            if (refreshedOffer == null)
            {
                throw new InvalidOperationException($"VacationOffer with ID {vacationOffer.VacationOfferId} not found.");
            }

            return refreshedOffer;
        }

        public async Task<List<VacationOffer>> LoadVacationOffersTreeByRequestIdAsync(VacationRequest vacationRequest)
        {
            List<VacationOffer> vacationOffers = await Context.VacationOffers
                .Include(o => o.FlightBooking)
                .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Legs)
                .Include(o => o.FlightBooking)
                        .ThenInclude(fb => fb.FlightRoutes)
                            .ThenInclude(fr => fr.Layovers)
                .Include(o => o.HotelBooking)
                .Where(o => o.VacationRequestId == vacationRequest.VacationRequestId)
                .ToListAsync();
            return vacationOffers;

        }
    }
}
