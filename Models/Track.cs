using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mutify.Models
{
    public class Track : BaseModel
    {
        [Required] [MaxLength(200)]
        public string Name { get; set; }
        public int? BlobFileId { get; set; }

        public List<Genre> Genres { get; set; }

        [ForeignKey("BlobFileId")]
        public BlobFile Content { get; set; }
    }
}