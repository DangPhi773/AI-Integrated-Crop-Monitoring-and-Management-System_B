using CMMS.BLL.Interfaces;
using CMMS.BLL.Mappings;
using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Tasks;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMMS.BLL.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly ISubTaskRepository _subTaskRepo;
        public SubTaskService(ISubTaskRepository subTaskRepo) => _subTaskRepo = subTaskRepo;

        public async Task<ApiResponse<SubTaskResponse>> CreateSubTaskAsync(SubTaskCreateRequest request)
        {
            var subTask = new SubTask
            {
                SubTaskId = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                TaskDetailId = request.TaskDetailId
            };
            await _subTaskRepo.AddAsync(subTask);
            await _subTaskRepo.SaveChangesAsync();

            return new ApiResponse<SubTaskResponse>
            {
                Success = true,
                Data = SubTaskMapper.ToResponse(subTask)
            };
        }

        public async Task<ApiResponse<IEnumerable<SubTaskResponse>>> GetSubTasksByTaskAsync(Guid taskDetailId)
        {
            var data = await _subTaskRepo.GetAllByTaskDetailIdAsync(taskDetailId);
            var res = data.Select(SubTaskMapper.ToResponse);
            return new ApiResponse<IEnumerable<SubTaskResponse>> { Success = true, Data = res };
        }

        public async Task<ApiResponse<string>> UpdateSubTaskAsync(Guid id, SubTaskUpdateRequest request)
        {
            var subTask = await _subTaskRepo.GetByIdAsync(id);
            if (subTask == null) return new ApiResponse<string> { Success = false, Message = "Not found" };

            subTask.Title = request.Title;
            subTask.Description = request.Description;

            _subTaskRepo.Update(subTask);
            return await _subTaskRepo.SaveChangesAsync()
                ? new ApiResponse<string> { Success = true, Message = "Updated" }
                : new ApiResponse<string> { Success = false, Message = "Update failed" };
        }

        public async Task<ApiResponse<string>> DeleteSubTaskAsync(Guid id)
        {
            var subTask = await _subTaskRepo.GetByIdAsync(id);
            if (subTask == null) return new ApiResponse<string> { Success = false, Message = "Not found" };

            _subTaskRepo.Delete(subTask);
            await _subTaskRepo.SaveChangesAsync();
            return new ApiResponse<string> { Success = true, Message = "Deleted" };
        }
    }
}
