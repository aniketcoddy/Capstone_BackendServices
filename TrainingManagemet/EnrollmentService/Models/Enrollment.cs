namespace EnrollmentService.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public bool Status { get; set; }
    }
}
