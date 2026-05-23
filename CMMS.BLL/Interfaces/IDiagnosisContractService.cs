using CMMS.DAL.DTOs.Auth;
using CMMS.DAL.DTOs.Payment;

namespace CMMS.BLL.Interfaces;

public interface IDiagnosisContractService
{
    Task<ApiResponse<ContractResponse>> CreateAsync(CreateContractRequest request, Guid ownerId);
    Task<ApiResponse<ContractResponse>> GetByIdAsync(Guid id, Guid userId, string role);
    Task<ApiResponse<IEnumerable<ContractResponse>>> GetMyContractsAsync(Guid userId, string role);
    Task<ApiResponse<IEnumerable<ContractResponse>>> GetAllAsync();
    Task<ApiResponse<ContractResponse>> UpdateAsync(Guid id, UpdateContractRequest request, Guid ownerId);
    Task<ApiResponse<string>> TerminateAsync(Guid id, Guid ownerId);
}
