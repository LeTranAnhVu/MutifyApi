using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mutify.Models
{
    public class Genre: BaseModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public virtual List<Track> Tracks { get; set; }
    }
}