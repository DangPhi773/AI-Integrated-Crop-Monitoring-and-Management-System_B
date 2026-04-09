using System;
using System.Threading.Tasks;

namespace CMMS.BLL.Interfaces
{
    public interface INotificationService
    {
        Task NotifyNewWorkerAsync(Guid workerId);
        Task NotifyNewReportAsync(Guid reportId);
    }
}
