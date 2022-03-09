using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Library.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Book> Books { get; set; }
    }
}
