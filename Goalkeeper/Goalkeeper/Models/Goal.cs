namespace Goalkeeper.Models
{
    public class Goal
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public NameId Area { get; set; }
        public NameId DateRange { get; set; }
        public int VoteCount { get; set; }
    }

    public struct NameId
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}