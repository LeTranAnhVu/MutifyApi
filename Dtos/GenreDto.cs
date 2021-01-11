using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Mutify.Models;

namespace Mutify.Dtos
{
    public class GenreDto
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public static Expression<Func<Genre, GenreDto>> AsDto = track => new GenreDto
        {
            Id = track.Id,
            Name = track.Name,
        };
    }
}