using GotorzProjectMain.Models;

namespace GotorzProjectMain.Services
{
    public interface IVacationRequestTreeService
    {
        Task<VacationRequest> LoadVacationRequestTreeByIdAsync(int vacationRequestId);
        Task<VacationRequest> ReloadVacationRequestTreeAsync(VacationRequest vacationRequest);
        Task<VacationOffer> LoadVacationOfferTreeAsync(VacationOffer vacationOffer);
        Task<List<VacationOffer>> LoadVacationOffersTreeByRequestIdAsync(VacationRequest vacationRequest);
    }
}
