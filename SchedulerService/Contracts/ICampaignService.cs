using SchedulerService.Models;

namespace SchedulerService.Contracts
{
    public interface ICampaignService
    {
        Task ScheduleAndSendCampaignsAsync();
    }
}
