namespace CMMS.BLL.Configuration
{
    public class EmailSettings
    {
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = "CMMS Smart Farm System";
        public string SenderPassword { get; set; } = string.Empty;
    }
}
