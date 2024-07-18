using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnrollmentService.Data;
using EnrollmentService.Models;

namespace EnrollmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentsContext _context;

        public EnrollmentsController(EnrollmentsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
        {
            return await _context.Enrollments.ToListAsync();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollmentsByUser(int userId)
        {
            return await _context.Enrollments.Where(e => e.UserId == userId && e.Status).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnrollments), new { id = enrollment.Id }, enrollment);
        }

        [HttpGet("active")]
        public ActionResult<IEnumerable<Enrollment>> GetActiveEnrollments()
        {
            var enrollments = _context.Enrollments.Where(e => e.Status == true).ToList();
            return Ok(enrollments);
        }

        [HttpPatch("{id}")]
        public IActionResult RejectEnrollment(int id)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            enrollment.Status = false;
            _context.SaveChanges();

            return NoContent();
        }
    }

}

