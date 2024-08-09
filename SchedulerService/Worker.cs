using SchedulerService.Contracts;
using SchedulerService.Services;

namespace SchedulerService
{
    public class Worker : BackgroundService
    {
        private readonly ICampaignService _campaignService;

        public Worker(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _campaignService.ScheduleAndSendCampaignsAsync();
        }
    }
}
