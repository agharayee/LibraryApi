using AutoMapper;
using Library.Data.Entities;
using LibraryAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Book, AddBookDto>().ReverseMap();
            CreateMap<Book, BookToReturnDto>().ForMember(des => des.BookId, opt => opt.MapFrom(scr => scr.Id)).ReverseMap();
            CreateMap<Book, BookToReturnWIthDetails>().ReverseMap();
        }
    }
}
