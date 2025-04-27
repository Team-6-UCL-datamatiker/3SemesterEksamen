using GotorzProjectMain.Models;

namespace GotorzProjectMain.InputModels.VacationRequestInputModels

{
    public class EditVacationRequestInputModel : VacationRequestBaseInputModel
    {
        public Status Status { get; set; } = Status.PendingRequest;

    }
}
