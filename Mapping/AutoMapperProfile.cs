using AutoMapper;
using Ecommerce_Webapi.DTOs;
using Ecommerce_Webapi.DTOs.ProductDTO;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Models.UserModel;

namespace Ecommerce_Webapi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Users, UserDTO>().ReverseMap();
            CreateMap<Products, ProductDTO>().ReverseMap();
        }
    }
}
