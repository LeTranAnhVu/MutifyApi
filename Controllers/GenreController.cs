using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutify.Dtos;
using Mutify.Models;

namespace Mutify.Controllers
{
    [Route("api/genres")]
    public class GenreController : BaseController<Genre, GenreDto>
    {
        protected override DbSet<Genre> _dbSet => _context.Genres;
        protected override Expression<Func<Genre, GenreDto>> _asDto => GenreDto.AsDto;

        public GenreController(MutifyContext mutifyContext, IMapper mapper) : base(mutifyContext, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDto>>> Get()
        {
            return await _GetAll();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetOne(int id)
        {
            var genre = await _GetOneById(id);
            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> Post(GenreDto dto)
        {
            var genre = new Genre();
            _mapper.Map(dto, genre);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new {id = genre.Id}, genre);
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