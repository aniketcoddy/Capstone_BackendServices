using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnrollmentService.Data;
using EnrollmentService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Return enrollments with status true, false, or null
            return await _context.Enrollments.Where(e => e.UserId == userId).ToListAsync();
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
        public IActionResult PatchEnrollment(int id, [FromBody] EnrollmentStatusUpdate update)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            enrollment.Status = update.Status;
            _context.SaveChanges();

            return NoContent();
        }

        public class EnrollmentStatusUpdate
        {
            public bool? Status { get; set; }
        }


        [HttpGet("counts")]
        public async Task<ActionResult<object>> GetEnrollmentCounts()
        {
            var totalEnrollments = await _context.Enrollments.CountAsync();
            var approvedEnrollments = await _context.Enrollments.CountAsync(e => e.Status == true);
            var notApprovedEnrollments = await _context.Enrollments.CountAsync(e => e.Status == false);

            return Ok(new
            {
                Total = totalEnrollments,
                Approved = approvedEnrollments,
                NotApproved = notApprovedEnrollments
            });
        }

        // New endpoints to fetch enrollments by status
        [HttpGet("requested")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetRequestedEnrollments()
        {
            // Return all enrollments including those with null status
            return await _context.Enrollments.ToListAsync();
        }

        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetApprovedEnrollments()
        {
            return await _context.Enrollments.Where(e => e.Status == true).ToListAsync();
        }

        [HttpGet("notApproved")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetNotApprovedEnrollments()
        {
            return await _context.Enrollments.Where(e => e.Status == false).ToListAsync();
        }
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetPendingEnrollments()
        {
            // Return only enrollments with status null
            var enrollments = await _context.Enrollments.Where(e => e.Status == null).ToListAsync();
            return Ok(enrollments);
        }

    }
}
