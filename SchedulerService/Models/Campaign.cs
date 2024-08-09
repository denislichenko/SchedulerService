namespace SchedulerService.Models
{
    public class Campaign
    {
        public string Template { get; set; }

        public Func<Customer, bool> Condition { get; set; }

        public TimeSpan ScheduledTime { get; set; }

        public int Priority { get; set; }

        public DateTime GetScheduledDateTime()
        {
            DateTime todayScheduled = DateTime.Today.Add(ScheduledTime);

            // If the scheduled time is in the past, schedule it for the next day
            if (todayScheduled <= DateTime.Now)
            {
                todayScheduled = todayScheduled.AddDays(1);
            }

            return todayScheduled;
        }
    }
}
