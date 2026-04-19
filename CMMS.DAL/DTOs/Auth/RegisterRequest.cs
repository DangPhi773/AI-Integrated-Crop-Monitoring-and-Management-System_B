namespace CMMS.DAL.DTOs.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        public string TargetRole { get; set; }
    }
}
