using AutoMapper;
using Ecommerce_Webapi.Data.UserDtOs;
using Ecommerce_Webapi.DTOs.ProductDTO;
using Ecommerce_Webapi.DTOs.WhishListDTO;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Models.UserModel;

namespace Ecommerce_Webapi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Users, UserDTO>().ReverseMap();
            CreateMap<Users, OutUsers>().ReverseMap();
            CreateMap<Products, ProductDTO>().ReverseMap();
            CreateMap<Products,ProductViewDTO>().ReverseMap();
            CreateMap<WhishList,AddWhishListDTO>().ReverseMap();
        }
    }
}
