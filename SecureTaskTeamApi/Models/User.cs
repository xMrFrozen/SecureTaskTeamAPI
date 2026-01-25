namespace SecureTaskTeamApi.Models
{
    public class User
    {
        // ABOUT USER
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = null!;

        // TASKS
        public List<TaskItem> Tasks { get; set; } = new();

    }
}
