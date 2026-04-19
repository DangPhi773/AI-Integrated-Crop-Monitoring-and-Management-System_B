using Moq;
using Xunit;
using System.Text.Json;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;
using CMMS.WebAPI.Controllers;
using CMMS.BLL.Interfaces;
using CMMS.DAL.DTOs.Beds;
using CMMS.DAL.DTOs.Auth; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMMS.Tests.Unit.Controllers
{
    public class BedsControllerTests
    {
        private readonly Mock<IBedService> _mockBedService;
        private readonly BedsController _controller;
        private readonly ITestOutputHelper _output;

        public BedsControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _mockBedService = new Mock<IBedService>();
            _controller = new BedsController(_mockBedService.Object);
        }

        private void PrintJson(object result, string title)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            _output.WriteLine($"\n===== ACTUAL JSON FOR {title} =====");
            _output.WriteLine(JsonSerializer.Serialize(result, options));
            _output.WriteLine("====================================\n");
        }

        // ================== CREATE BED ==================
        [Fact]
        public async Task Create_Success()
        {
            var req = new BedRequest { BedName = "luong A1", PlotId = Guid.NewGuid() };
            var fakeResponse = new ApiResponse<string> { Success = true, Message = "Tạo luong thành công", Data = "New Bed Created" };

            _mockBedService.Setup(s => s.CreateBedAsync(It.IsAny<BedRequest>())).ReturnsAsync(fakeResponse);

            var result = await _controller.Create(req);
            var okResult = Assert.IsType<OkObjectResult>(result);
            PrintJson(okResult.Value, "Create_Success");
        }

        [Fact]
        public async Task Create_Fail_Duplicate()
        {
            var req = new BedRequest { BedName = "Trùng Tên" };
            var fakeResponse = new ApiResponse<string> { Success = false, Message = "Tên luong đã tồn tại" };

            _mockBedService.Setup(s => s.CreateBedAsync(It.IsAny<BedRequest>())).ReturnsAsync(fakeResponse);

            var result = await _controller.Create(req);
            Assert.IsType<BadRequestObjectResult>(result);
            PrintJson(((BadRequestObjectResult)result).Value, "Create_Fail_Duplicate");
        }

        [Fact] 
        public async Task Create_Fail_InvalidPlot()
        {
            var req = new BedRequest { BedName = "luong Mới", PlotId = Guid.Empty };
            var fakeResponse = new ApiResponse<string>
            {
                Success = false,
                Message = "Không tìm thấy lô đất (PlotId) yêu cầu"
            };

            _mockBedService.Setup(s => s.CreateBedAsync(It.IsAny<BedRequest>())).ReturnsAsync(fakeResponse);

            var result = await _controller.Create(req);
            Assert.IsType<BadRequestObjectResult>(result);
            PrintJson(((BadRequestObjectResult)result).Value, "Create_Fail_InvalidPlot");
        }

        // ================== UPDATE BED ==================
        [Fact]
        public async Task Update_Success()
        {
            var id = Guid.NewGuid();
            var req = new BedRequest { BedName = "luong Update" };
            var fakeResponse = new ApiResponse<string> { Success = true, Message = "Cập nhật thành công" };

            _mockBedService.Setup(s => s.UpdateBedAsync(id, It.IsAny<BedRequest>())).ReturnsAsync(fakeResponse);

            var result = await _controller.Update(id, req);
            Assert.IsType<OkObjectResult>(result);
            PrintJson(((OkObjectResult)result).Value, "Update_Success");
        }

        // ================== DELETE BED ==================
        [Fact]
        public async Task Delete_Success()
        {
            var id = Guid.NewGuid();
            var fakeResponse = new ApiResponse<string> { Success = true, Message = "Xóa luong thành công" };

            _mockBedService.Setup(s => s.DeleteBedAsync(id)).ReturnsAsync(fakeResponse);

            var result = await _controller.Delete(id);
            Assert.IsType<OkObjectResult>(result);
            PrintJson(((OkObjectResult)result).Value, "Delete_Success");
        }

        [Fact]
        public async Task Delete_Fail_InUse()
        {
            var id = Guid.NewGuid();
            var fakeResponse = new ApiResponse<string> { Success = false, Message = "luong đang có cây trồng" };

            _mockBedService.Setup(s => s.DeleteBedAsync(id)).ReturnsAsync(fakeResponse);

            var result = await _controller.Delete(id);
            Assert.IsType<BadRequestObjectResult>(result);
            PrintJson(((BadRequestObjectResult)result).Value, "Delete_Fail_InUse");
        }
    }
}