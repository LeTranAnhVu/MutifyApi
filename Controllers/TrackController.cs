using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutify.Dtos;
using Mutify.Models;

namespace Mutify.Controllers
{
    [Route("api/tracks")]
    public class TrackController : BaseController<Track, TrackDto>
    {
        protected override Expression<Func<Track, TrackDto>> _asDto => TrackDto.AsDto;
        protected override DbSet<Track> _dbSet => _context.Tracks;

        public TrackController(MutifyContext context, IMapper mapper) : base(context, mapper)
        {
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<TrackDto>>> Get()
        {
            return await _GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetOne(int id)
        {
            var track = await _GetOneById(id);
            if (track == null)
            {
                return NotFound();
            }

            Console.WriteLine(track.Genres);
            return Ok(track);
        }

        [HttpPost]
        public async Task<ActionResult<Track>> Post(TrackDto trackDto)
        {
            var genres = await _context.Genres.Where(genre => trackDto.GenreIds.Contains(genre.Id)).ToListAsync();
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

        #region Audio

        [HttpPost("{trackId}/upload-audio")]
        public async Task<ActionResult<Track>> UploadAudio(IFormFile file, int trackId)
        {
            var contentType = file.ContentType;
            var name = file.FileName;
            var size = file.Length;
            var now = (DateTimeOffset) DateTime.UtcNow;
            var timestamp = now.ToString("yyyyMMddHHmmssfff");

            var byteFileName = Encoding.UTF8.GetBytes(name + timestamp);
            var md5 = new HMACMD5();
            var hash = md5.ComputeHash(byteFileName);
            var hashFileName = BitConverter.ToString(hash).Replace("-", "").ToLower() + ".mp3";
            var root = Constants.RootPath;
            var resourcePath = Path.Join(root, Constants.Resource.ResourceFolder);
            var audioPath = Path.Join(resourcePath, Constants.Resource.AudioFolder);
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    var bytes = new byte[stream.Length];

                    var content256Hash = BitConverter.ToString(SHA256.HashData(bytes)).Replace("-", "").ToLower();

                    await stream.ReadAsync(bytes);

                    using (var writer =
                        new BinaryWriter(System.IO.File.Create(Path.Join(audioPath, hashFileName))))
                    {
                        writer.Write(bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Ok(new
            {
                Message = "Upload success"
            });
        }

        #endregion

        private async Task<bool> TrackExists(int id)
        {
            return await _context.Tracks.AnyAsync(track => track.Id == id);
        }
    }
}