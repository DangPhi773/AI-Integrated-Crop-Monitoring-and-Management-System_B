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
    }
}
