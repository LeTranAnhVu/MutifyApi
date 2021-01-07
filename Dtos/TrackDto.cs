using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mutify.Dtos
{
    public class TrackDto
    {
        public int? Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public List<int> Genres { get; set; }
    }
}