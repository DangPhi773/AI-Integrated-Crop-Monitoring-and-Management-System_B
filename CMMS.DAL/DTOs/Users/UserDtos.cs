using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.DAL.DTOs.Users
{
    public class WorkerRequest
    {
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public Guid? RoleId { get; set; } // ID của role Worker trong DB
    }

    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? Fullname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? RoleName { get; set; } // Hiển thị "Worker" thay vì GUID
    }
}
