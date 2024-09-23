using AutoMapper;
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs.CategoryDTO;
using Ecommerce_Webapi.DTOs.ProductDTO;
using Ecommerce_Webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webapi.Services.ProductService
{
    public class ProductService : IProductService
    {
        private AppDbContext _context;
        private IMapper _mapper;
        public ProductService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProduct()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                var prod = _mapper.Map<IEnumerable<ProductDTO>>(products);
                return prod;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(pro => pro.Id == id);
                if (product == null)
                {
                    return null;
                }
                var prod = _mapper.Map<ProductDTO>(product);
                return prod;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }
        public async Task<IEnumerable<ProductDTO>> GetProductByCat(CategoryDTO category)
        {
            try
            {
                var cat = await _context.Categories.Select(c => c.CategoryName == category.CategoryName);
                if (cat==null)
                {
                    return null;
                }
                var prod = await _context.

            }
        }


    }
}
