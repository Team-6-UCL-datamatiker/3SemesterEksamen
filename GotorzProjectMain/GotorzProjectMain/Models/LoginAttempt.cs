namespace GotorzProjectMain.Models
{
    public class LoginAttempt
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime TimeOfAttempt { get; set; }
        public string? IPAddress { get; set; }
        public string? FailureReason { get; set; }
    }
}
