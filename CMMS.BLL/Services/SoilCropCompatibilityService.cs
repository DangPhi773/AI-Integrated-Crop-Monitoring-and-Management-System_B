using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Crops;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class SoilCropCompatibilityService : ISoilCropCompatibilityService   
    {
        private readonly ISoilCropCompatibilityRepository _repo;
        public SoilCropCompatibilityService(ISoilCropCompatibilityRepository repo) => _repo = repo;

        public async Task<ApiResponse<IEnumerable<SoilCropCompatibilityResponse>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return new ApiResponse<IEnumerable<SoilCropCompatibilityResponse>>
            {
                Success = true,
                Data = SoilCropCompatibilityMapper.ToResponseList(list)
            };
        }

        public async Task<ApiResponse<SoilCropCompatibilityResponse>> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(id);

                if (entity == null)
                {
                    return new ApiResponse<SoilCropCompatibilityResponse>
                    {
                        Success = false,
                        Message = "Không tìm thấy dữ liệu tương thích đất - cây trồng này."
                    };
                }

                var data = SoilCropCompatibilityMapper.ToResponse(entity);

                return new ApiResponse<SoilCropCompatibilityResponse>
                {
                    Success = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<SoilCropCompatibilityResponse>
                {
                    Success = false,
                    Message = "Lỗi khi lấy chi tiết dữ liệu",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<string>> CreateAsync(SoilCropCompatibilityRequest request)
        {
            var existing = await _repo.GetBySoilAndCropAsync(request.SoilId, request.CropId);
            if (existing != null) return new ApiResponse<string> { Success = false, Message = "Cây trồng này đã có cấu hình tương thích với loại đất này rồi." };

            var entity = new SoilCropCompatibility
            {
                ComptId = Guid.NewGuid(),
                SoilId = request.SoilId,
                CropId = request.CropId,
                Compatibility = request.Compatibility,
                Note = request.Note
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Thêm thành công." };
        }

        public async Task<ApiResponse<string>> UpdateAsync(Guid id, SoilCropCompatibilityRequest request)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy dữ liệu." };

            entity.Compatibility = request.Compatibility;
            entity.Note = request.Note;

            _repo.Update(entity);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Cập nhật thành công." };
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return new ApiResponse<string> { Success = false, Message = "Không tìm thấy." };

            _repo.Delete(entity);
            await _repo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Xóa thành công." };
        }
    }
}
