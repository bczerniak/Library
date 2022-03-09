using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Dto
{
    public class BookDto
    {
        public int Id { get; set; }

        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Podaj tytuł książki")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Tytuł musi zawierać od 2 do 40 znaków")]
        public string Title { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Podaj autora książki")]
        [RegularExpression(@"^[a-zA-Z-\s]{2,50}$", ErrorMessage = "Niedozwolone znaki lub nieodpowiednia ilość znaków (2-50)")]
        public string Author { get; set; }

        [Display(Name = "Data wydania")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Podaj datę wydania książki")]
        public DateTime ReleaseDate { get; set; }
    }
}
