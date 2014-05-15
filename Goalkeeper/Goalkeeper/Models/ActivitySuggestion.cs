namespace Goalkeeper.Models
{
    public class ActivitySuggestion
    {
        public ActivitySuggestion()
        {
            SuggestionState = ActivitySuggestionState.Open;
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public NameId Goal { get; set; }
        public ActivitySuggestionState SuggestionState { get; set; }
    }
}