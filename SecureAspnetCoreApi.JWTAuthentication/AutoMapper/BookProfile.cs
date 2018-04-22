using AutoMapper;
using SecureAspnetCoreApi.JWTAuthentication.Data;
using SecureAspnetCoreApi.JWTAuthentication.Models;

namespace SecureAspnetCoreApi.JWTAuthentication.AutoMapper
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
