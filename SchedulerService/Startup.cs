using CsvHelper;
using CsvHelper.Configuration;
using SchedulerService.Contracts;
using SchedulerService.Models;
using SchedulerService.Services;
using System.Globalization;

namespace SchedulerService
{
    public static class Startup
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var customers = LoadCustomers("Resources/customers.csv");
                    var campaigns = ConfigureCampaigns();

                    services.AddSingleton(customers);
                    services.AddSingleton(campaigns);
                    services.AddSingleton<IFileSendService, FileSendService>();
                    services.AddSingleton<ICampaignService, CampaignService>();
                    services.AddHostedService<Worker>(); // Use Worker for scheduling tasks
                });

        private static List<Customer> LoadCustomers(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null, // Disable missing field validation
            });
            return csv.GetRecords<Customer>().ToList();
        }

        private static List<Campaign> ConfigureCampaigns()
        {
            return new List<Campaign>
            {
                new Campaign { Template = "TemplateA.html", Condition = c => c.Gender == "Male", ScheduledTime = new TimeSpan(10, 15, 0), Priority = 1 },
                new Campaign { Template = "TemplateB.html", Condition = c => c.Age > 45, ScheduledTime = new TimeSpan(10, 05, 0), Priority = 2 },
                new Campaign { Template = "TemplateC.html", Condition = c => c.City == "New York", ScheduledTime = new TimeSpan(10, 10, 0), Priority = 5 },
                new Campaign { Template = "TemplateA.html", Condition = c => c.Deposit > 100, ScheduledTime = new TimeSpan(10, 15, 0), Priority = 3 },
                new Campaign { Template = "TemplateC.html", Condition = c => c.NewCustomer, ScheduledTime = new TimeSpan(10, 05, 0), Priority = 4 }
            };
        }
    }
}
