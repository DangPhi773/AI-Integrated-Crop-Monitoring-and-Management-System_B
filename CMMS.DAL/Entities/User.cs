using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMS.DAL.Entities;

[Table("User")]
public partial class User
{
    [Key]
    public Guid UserId { get; set; }

    public Guid? RoleId { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [MinLength(8, ErrorMessage = "Mật khẩu phải từ 8 ký tự trở lên.")]
    [MaxLength(255)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Mật khẩu phải có chữ hoa, chữ thường, số và ký tự đặc biệt.")]
    public string? Password { get; set; }

    public string? HashPassword { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống.")]
    [MaxLength(255)]
    public string? Fullname { get; set; }

    [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng số 0.")]
    [MaxLength(10)]
    public string? PhoneNumber { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<RecommendationTask> RecommendationTaskAssignedToWorkers { get; set; } = new List<RecommendationTask>();
    public virtual ICollection<RecommendationTask> RecommendationTaskCreatedByOwners { get; set; } = new List<RecommendationTask>();
    public virtual ICollection<RecommendationTaskDetail> RecommendationTaskDetails { get; set; } = new List<RecommendationTaskDetail>();
    public virtual ICollection<Report> CreatedReports { get; set; } = new List<Report>();
    public virtual ICollection<Report> OwnedReports { get; set; } = new List<Report>();
    public virtual ICollection<WorkerSchedule> WorkerSchedules { get; set; } = new List<WorkerSchedule>();
}