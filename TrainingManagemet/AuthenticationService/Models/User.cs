namespace AuthenticationService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }  // Changed type to int for role
        public bool Status { get; set; }  // Changed type to bool for status
        public string PasswordHash { get; set; }
    }
}
