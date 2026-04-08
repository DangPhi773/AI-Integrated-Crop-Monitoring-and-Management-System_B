using CMMS.BLL.Interfaces;

namespace CMMS.BLL.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public (string subject, string htmlBody) BuildWelcomeWorkerEmail(string workerName, string workerEmail)
        {
            var subject = "Chào mừng bạn đến với CMMS Smart Farm";
            var htmlBody = $@"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'></head>
<body style='font-family: Arial, sans-serif; background:#f6f8fa; padding:24px;'>
  <div style='max-width:560px; margin:auto; background:#ffffff; border-radius:8px; padding:32px; box-shadow:0 2px 8px rgba(0,0,0,0.08);'>
    <h2 style='color:#2e7d32; margin-top:0;'>Chào mừng {workerName}!</h2>
    <p>Tài khoản Worker của bạn trên hệ thống <strong>CMMS Smart Farm</strong> đã được tạo thành công.</p>
    <table style='width:100%; margin:16px 0; border-collapse:collapse;'>
      <tr><td style='padding:8px; background:#f1f3f5;'><strong>Email đăng nhập</strong></td><td style='padding:8px;'>{workerEmail}</td></tr>
    </table>
    <p>Vui lòng liên hệ quản lý nông trại để nhận mật khẩu đăng nhập (nếu chưa có).</p>
    <p style='color:#666; font-size:12px; margin-top:32px;'>Đây là email tự động, vui lòng không trả lời.</p>
  </div>
</body>
</html>";
            return (subject, htmlBody);
        }

        public (string subject, string htmlBody) BuildNewReportEmail(string reportTitle, string workerName, string submitDate)
        {
            var subject = $"Báo cáo mới: {reportTitle}";
            var htmlBody = $@"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'></head>
<body style='font-family: Arial, sans-serif; background:#f6f8fa; padding:24px;'>
  <div style='max-width:560px; margin:auto; background:#ffffff; border-radius:8px; padding:32px; box-shadow:0 2px 8px rgba(0,0,0,0.08);'>
    <h2 style='color:#1565c0; margin-top:0;'>Có báo cáo mới cần xem xét</h2>
    <table style='width:100%; margin:16px 0; border-collapse:collapse;'>
      <tr><td style='padding:8px; background:#f1f3f5;'><strong>Tiêu đề</strong></td><td style='padding:8px;'>{reportTitle}</td></tr>
      <tr><td style='padding:8px; background:#f1f3f5;'><strong>Người gửi</strong></td><td style='padding:8px;'>{workerName}</td></tr>
      <tr><td style='padding:8px; background:#f1f3f5;'><strong>Ngày gửi</strong></td><td style='padding:8px;'>{submitDate}</td></tr>
    </table>
    <p>Vui lòng đăng nhập vào hệ thống CMMS Smart Farm để xem chi tiết báo cáo.</p>
    <p style='color:#666; font-size:12px; margin-top:32px;'>Đây là email tự động, vui lòng không trả lời.</p>
  </div>
</body>
</html>";
            return (subject, htmlBody);
        }
    }
}
