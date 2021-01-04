using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutify.Models;

namespace Mutify.Controllers
{
    [Route("api/tracks")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly MutifyContext _context;

        public TrackController(MutifyContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<Track>>> Get()
        {
            return await _context.Tracks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetOne(int id)
        {
            Track track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            return track;
        }

        [HttpPost]
        public async Task<ActionResult<Track>> Post(Track trackDto)
        {
            _context.Tracks.Add(trackDto);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new {id = trackDto.Id}, trackDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Track>> Put(int id, Track trackDto)
        {
            if (id != trackDto.Id)
            {
                return BadRequest();
            }
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            _context.Entry(trackDto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(trackDto);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            _context.Tracks.Remove(track);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong" + e);
                throw;
            }

            return NoContent();
        }
    }
}