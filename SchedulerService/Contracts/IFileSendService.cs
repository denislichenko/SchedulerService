namespace SchedulerService.Contracts
{
    public interface IFileSendService
    {
        Task SendAsync(string templateFileName, int customerId);
    }
}
