using System.ComponentModel.DataAnnotations;

namespace CMMS.DAL.DTOs.Reports.Requests
{
    public class CreateDiagnosisRequest
    {
        [Required(ErrorMessage = "Tên bệnh là bắt buộc")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Tên bệnh phải từ 1 đến 200 ký tự")]
        public string DiseaseName { get; set; } = null!;

        [Required(ErrorMessage = "Kết luận là bắt buộc")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Kết luận phải từ 1 đến 2000 ký tự")]
        public string Conclusion { get; set; } = null!;

        [Required(ErrorMessage = "Khuyến nghị là bắt buộc")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Khuyến nghị phải từ 1 đến 2000 ký tự")]
        public string RecommendedAction { get; set; } = null!;

        [StringLength(50, ErrorMessage = "SeverityLevel tối đa 50 ký tự")]
        public string? SeverityLevel { get; set; }
    }
}
