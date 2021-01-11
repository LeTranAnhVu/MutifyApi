using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mutify.Models
{
    public class Track : BaseModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public List<Genre> Genres { get; set; }
    }
}