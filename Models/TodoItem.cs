namespace todoproject_andrian.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string ActivitiesNo { get; set; }
        public TodoStatus Status { get; set; }
    }
    public enum TodoStatus
    {
        Unmarked,
        Marked,
        Done,
        Canceled
    }
}
