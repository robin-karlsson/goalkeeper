namespace Goalkeeper.Models
{
    public class Goal
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AreaId { get; set; }
        public string DateRangeId { get; set; }
        public int VoteCount { get; set; }
    }
}