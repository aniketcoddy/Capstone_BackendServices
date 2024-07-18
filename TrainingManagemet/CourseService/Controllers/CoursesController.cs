using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseService.Models;
using CourseService.Data;

namespace CourseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseContext _context;

        public CoursesController(CourseContext context)
        {
            _context = context;

            if (_context.Courses.Count() == 0)
            {
                // Create a new Course if collection is empty,
                // which means you can't delete all Courses.
                _context.Courses.Add(new Course { Name = "C#", Description = "C# course", Status = true, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) });
                _context.Courses.Add(new Course { Name = "Java", Description = "Java with OOPS", Status = true, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.Where(c => c.Status == true).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> SoftDeleteCourse(int id, [FromBody] Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.Status = false;

            _context.Entry(existingCourse).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // New endpoint to fetch courses with status=false
        [HttpGet("inactive")]
        public async Task<ActionResult<IEnumerable<Course>>> GetInactiveCourses()
        {
            return await _context.Courses.Where(c => c.Status == false).ToListAsync();
        }


        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
