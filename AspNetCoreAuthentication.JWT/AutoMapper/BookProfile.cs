using AutoMapper;
using AspNetCoreAuthentication.JWT.Data;
using AspNetCoreAuthentication.JWT.Models;

namespace AspNetCoreAuthentication.JWT.AutoMapper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookModel>()
                .ForMember(m => m.Url, opt => opt.ResolveUsing<BookUrlResolver>())
                .ReverseMap();
        }
    }
}
