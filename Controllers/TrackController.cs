using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutify.Dtos;
using Mutify.Models;

namespace Mutify.Controllers
{
    [Route("api/tracks")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly MutifyContext _context;
        private readonly IMapper _mapper;

        public TrackController(MutifyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            // Track track = await _context.Tracks.FindAsync(id);
            Track track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            var genres = track.Genres;

            return Ok(track);
        }

        [HttpPost]
        public async Task<ActionResult<Track>> Post(TrackDto trackDto)
        {
            var genres = await _context.Genres.Where(genre => trackDto.Genres.Contains(genre.Id)).ToListAsync();
            var track = new Track();
            _mapper.Map(trackDto, track);

            track.Genres = new List<Genre>();

            track.Genres.AddRange(genres);
            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOne), new {id = track.Id}, track);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Track>> Put(int id, Track trackDto)
        {
            if (id != trackDto.Id)
            {
                return BadRequest();
            }

            _context.Entry(trackDto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(trackDto);
            }
            catch (DbUpdateException)
            {
                if (!(await TrackExists(id)))
                {
                    return NotFound();
                }

                return BadRequest();
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



        private async Task<bool> TrackExists(int id)
        {
            return await _context.Tracks.AnyAsync(track => track.Id == id);
        }
    }
}