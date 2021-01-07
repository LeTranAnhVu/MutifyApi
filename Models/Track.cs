﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mutify.Models
{
    public class Track
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public virtual List<Genre> Genres { get; set; }
    }
}