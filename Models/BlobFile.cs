using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace Mutify.Models
{
    public class BlobFile : BaseModel
    {
        [Required]
        [MaxLength(64)]
        public string Hash { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(4000)]
        public string Directory { get; set; }

        [MaxLength(200)]
        public string Extension { get; set; }

        public ulong Size { get; set; }

    }
}