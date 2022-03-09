using AutoMapper;
using Library.Dto;
using Library.Models;

namespace Library.Mapping
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookDto>()
                .ReverseMap();
        }
    }
}