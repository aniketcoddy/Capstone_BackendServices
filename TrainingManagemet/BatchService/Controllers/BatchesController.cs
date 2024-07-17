using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BatchService.Data;
using BatchService.Models;

namespace BatchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchesController : ControllerBase
    {
        private readonly BatchContext _context;

        public BatchesController(BatchContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Batch>>> GetBatches()
        {
            return await _context.Batches.Where(b => b.Status == true).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Batch>> GetBatch(int id)
        {
            var batch = await _context.Batches.FindAsync(id);

            if (batch == null)
            {
                return NotFound();
            }

            return batch;
        }

        [HttpPost]
        public async Task<ActionResult<Batch>> PostBatch(Batch batch)
        {
            _context.Batches.Add(batch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBatch), new { id = batch.Id }, batch);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatch(int id, Batch batch)
        {
            if (id != batch.Id)
            {
                return BadRequest();
            }

            _context.Entry(batch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchExists(id))
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBatch(int id, Batch batch)
        {
            var existingBatch = await _context.Batches.FindAsync(id);
            if (existingBatch == null)
            {
                return NotFound();
            }

            existingBatch.Status = batch.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }

            batch.Status = false;  // Mark batch as inactive instead of deleting
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BatchExists(int id)
        {
            return _context.Batches.Any(e => e.Id == id);
        }
    }
}
