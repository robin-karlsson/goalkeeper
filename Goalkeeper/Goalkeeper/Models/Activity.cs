namespace Goalkeeper.Models
{
    public class Activity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public ActivityState ActivityState { get; set; }
        public NameId Performer { get; set; }
        public NameId Goal { get; set; }
    }
}