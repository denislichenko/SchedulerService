using SchedulerService.Contracts;
using SchedulerService.Models;

namespace SchedulerService.Services
{
    public class CampaignService : ICampaignService
    {
        public readonly IFileSendService _fileService;
        public readonly List<Customer> _customers;
        public readonly List<Campaign> _campaigns;
        public readonly List<Customer> _sendedCustomers;

        public CampaignService(
            IFileSendService fileService,
            List<Customer> customers,
            List<Campaign> campaigns)
        {
            _fileService = fileService;
            _customers = customers;
            _campaigns = campaigns.OrderBy(c => c.Priority).ToList();

            _sendedCustomers = new List<Customer>();
        }

        public async Task ScheduleAndSendCampaignsAsync()
        {
            var customersSentTo = new HashSet<int>();

            foreach (var campaign in _campaigns)
            {
                DateTime scheduledDateTime = campaign.GetScheduledDateTime();
                var delay = scheduledDateTime - DateTime.Now;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay);
                }

                var eligibleCustomers = _customers.Where(campaign.Condition).ToList();

                var tasks = new List<Task>();

                foreach (var customer in eligibleCustomers)
                {
                    if (!customersSentTo.Contains(customer.CUSTOMER_ID))
                    {
                        tasks.Add(_fileService.SendAsync(campaign.Template, customer.CUSTOMER_ID));
                        customersSentTo.Add(customer.CUSTOMER_ID);
                    }
                }

                await Task.WhenAll(tasks); // Wait for all tasks to complete before moving to the next campaign
                await Task.Delay(1000 * 60 * 30); // Wait 30 minutes before next campaign. Possible improvement: Move it to appsettings.json

            }
        }
    }
}
