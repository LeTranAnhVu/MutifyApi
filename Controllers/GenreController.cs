using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutify.Models;

namespace Mutify.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController: ControllerBase
    {
        private readonly MutifyContext _context;

        public GenreController(MutifyContext mutifyContext)
        {
            _context = mutifyContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await _context.Genres.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetOne(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> Post(Genre dto)
        {
            _context.Genres.Add(dto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new {id = dto.Id}, dto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}