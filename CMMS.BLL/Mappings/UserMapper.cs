using CMMS.DAL.DTOs.Users;
using CMMS.DAL.Entities;

namespace CMMS.BLL.Mappings
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(User u) => new()
        {
            UserId = u.UserId,
            Email = u.Email,
            Fullname = u.Fullname,
            PhoneNumber = u.PhoneNumber,
            Status = u.Status,
            CreatedAt = u.CreatedAt,
            RoleName = u.Role?.RoleName
        };

        public static User ToEntity(StaffRequest request)
        {
            return new User
            {
                Email = request.Email,
                Fullname = request.Fullname,
                PhoneNumber = request.PhoneNumber,
                RoleId = request.RoleId,
                Status = request.Status ?? "Active",

                HashPassword = !string.IsNullOrEmpty(request.Password)
                               ? BCrypt.Net.BCrypt.HashPassword(request.Password)
                               : null
            };
        }
    }
}
