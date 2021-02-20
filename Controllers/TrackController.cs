using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeTypes;
using Mutify.Dtos;
using Mutify.Helpers;
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

            return CreatedAtAction(nameof(GetOne), new {id = track.Id},  await _GetOneById(track.Id));
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
            var audioPath = PathHelper.GetAudioPath();
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
                        new BinaryWriter(System.IO.File.Create(Path.Combine(audioPath, hashFileName))))
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

        [HttpGet("{trackId}/download-audio/{audioId}")]
        public async Task<ActionResult> DownloadAudio(int trackId, int audioId)
        {
            var name = "52e48bb41d7b21e348d284172fbc6468.mp3";
            var names = name.Split('.');
            var ext = names[names.Length - 1];
            var audioPath = PathHelper.GetAudioPath();
            var filePath = Path.Combine(audioPath, name);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memStream = new MemoryStream();
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                await file.CopyToAsync(memStream);
                memStream.Position = 0;

                return File(memStream, MimeTypeMap.GetMimeType(ext));
            }

            return Ok();
        }

        #endregion

        private async Task<bool> TrackExists(int id)
        {
            return await _context.Tracks.AnyAsync(track => track.Id == id);
        }
    }
}