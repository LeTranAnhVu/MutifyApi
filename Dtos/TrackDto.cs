using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Mutify.Models;

namespace Mutify.Dtos
{
    public class TrackDto
    {
        public int? Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public List<Genre> Genres { get; set; }

        public List<int>? GenreIds { get; set; }

        public static Expression<Func<Track, TrackDto>> AsDto = track => new TrackDto
        {
            Id = track.Id,
            Name = track.Name,
            Genres = track.Genres.Select(ge => new Genre
            {
                Id = ge.Id,
                Name = ge.Name
            }).ToList()
        };
    }
}