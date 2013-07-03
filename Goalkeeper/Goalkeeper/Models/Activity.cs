namespace Goalkeeper.Models
{
    public class Activity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ActivityState ActivityState { get; set; }
        public string PerformerId { get; set; }
        public string GoalId { get; set; }
    }
}