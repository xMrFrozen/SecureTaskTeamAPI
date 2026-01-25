namespace SecureTaskTeamApi.Models
{
    public class TaskItem
    {
        // ABOUT TASK

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        // TASK OWNER
        public int UserID { get; set; }
        public User User { get; set; } = null!;
    }
}
