using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Author { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}