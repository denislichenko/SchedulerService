using SchedulerService.Contracts;
using SchedulerService.Models;

namespace SchedulerService.Services
{
    public class FileSendService : IFileSendService
    {
        private static readonly SemaphoreSlim Semaphore = new(1, 1);

        public async Task SendAsync(string templateFileName, int customerId)
        {
            string templatePath = Path.Combine("Resources/Templates", templateFileName);
            string templateContent = await File.ReadAllTextAsync(templatePath);

            string date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "sendedFiles");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = Path.Combine(directoryPath, $"send_{date}_customer{customerId}.html");  // It may be better use GUID to get unique file name.

            await Semaphore.WaitAsync();
            try
            {
                await File.WriteAllTextAsync(fileName, templateContent);
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}
