using Moq;
using Xunit;
using System.Text.Json;
using Xunit.Abstractions;
using CMMS.BLL.Services;
using CMMS.DAL.Interfaces;
using CMMS.DAL.Entities;
using CMMS.DAL.DTOs.Auth;
using CMMS.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CMMS.Tests.Unit.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IEmailService> _mockEmail;
        private readonly AuthService _authService;
        private readonly ITestOutputHelper _output;

        public AuthServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _mockRepo = new Mock<IUserRepository>();
            _mockEmail = new Mock<IEmailService>();

            var myConfiguration = new Dictionary<string, string>
            {
                {"JwtSettings:SecretKey", "ChuoiKeySieuBaoMatCuaThuan2026_@_v3"},
                {"JwtSettings:Issuer", "https://localhost:7021/"},
                {"JwtSettings:Audience", "https://localhost:7021/"},
                {"AppSettings:Secret", "ChuoiKeySieuBaoMatCuaThuan2026_@_v3"} 
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _authService = new AuthService(_mockRepo.Object, _mockEmail.Object, configuration);
        }

        private void PrintJson(object result, string title)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            _output.WriteLine($"\n===== ACTUAL JSON FOR {title} =====");
            _output.WriteLine(JsonSerializer.Serialize(result, options));
            _output.WriteLine("====================================\n");
        }

        [Fact] // UTCID01 - Login Success
        public async System.Threading.Tasks.Task UTCID01_Login_Success()
        {
            var testPassword = "123456@Aa";
            var request = new LoginRequest { Email = "thaithuan4323123@gmail.com", Password = testPassword };

            var fakeRole = new Role { RoleId = Guid.NewGuid(), RoleName = "Worker" };
            var fakeUser = new User
            {
                UserId = Guid.Parse("10c82efa-fc0c-4b8a-84b9-4f684da17adb"),
                Email = request.Email,
                Fullname = "Nguyễn Thái Thuận",
                HashPassword = BCrypt.Net.BCrypt.HashPassword(testPassword),
                Status = "ACTIVE",
                Role = fakeRole
            };

            _mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);

            var result = await _authService.LoginAsync(request);
            PrintJson(result, "UTCID01 (Login Success)");

            Assert.True(result.Success);
        }

        [Fact] // UTCID02 - Wrong Password
        public async System.Threading.Tasks.Task UTCID02_Login_WrongPassword()
        {
            var request = new LoginRequest { Email = "thaithuan4323123@gmail.com", Password = "wrong_password" };
            var fakeUser = new User
            {
                Email = request.Email,
                HashPassword = BCrypt.Net.BCrypt.HashPassword("123456@Aa"),
                Status = "ACTIVE"
            };
            _mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);

            var result = await _authService.LoginAsync(request);
            PrintJson(result, "UTCID02");
            Assert.False(result.Success);
        }

        [Fact] // UTCID03 - Register Success
        public async System.Threading.Tasks.Task UTCID03_Register_Success()
        {
            var request = new RegisterRequest
            {
                Email = "newuser@gmail.com",
                Fullname = "Thái Thuận",
                Password = "123@Aa",
                TargetRole = "Worker"
            };
            _mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
            _mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

            var result = await _authService.RegisterAsync(request);
            PrintJson(result, "UTCID03");
            Assert.True(result.Success);
        }
    }
}