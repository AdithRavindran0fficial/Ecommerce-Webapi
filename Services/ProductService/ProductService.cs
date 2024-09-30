using AutoMapper;
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs.CategoryDTO;
using Ecommerce_Webapi.DTOs.ProductDTO;
using Ecommerce_Webapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webapi.Services.ProductService
{
    public class ProductService : IProductService
    {
        private AppDbContext _context;
        private IMapper _mapper;
        private ILogger _logger;
        public ProductService(AppDbContext context,IMapper mapper,ILogger<ProductService>logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ProductViewDTO>> GetAllProduct()
        {
            try
            {
                var product =await _context.Products.Include(p=>p.Category).Where(pr=>pr.status==true).ToListAsync();
                if (product.Any())
                {
                    var items = _mapper.Map<IEnumerable<ProductViewDTO>>(product);


                    var products = product.Select(p =>
                    new ProductViewDTO
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        Img = p.Img,
                        Category = p.Category.CategoryName,
                        Price = p.Price
                    }

                    ).ToList();
                    return products ;
                }
                return new List<ProductViewDTO>();

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<ProductViewDTO> GetProductById(int id)
        {
            try
            {
                var pr = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(pr => pr.Id == id);
                if (pr == null || pr.status == false) 
                {
                    return new ProductViewDTO();
                }
                
                var product = _mapper.Map<ProductViewDTO>(pr);
                return product;
                

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
           
        }
        public async Task<IEnumerable<ProductViewDTO>> GetProductByCat([FromBody]CategoryDTO category)
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).Where(p => p.Category.CategoryName == category.CategoryName && p.status==true).ToListAsync();
                if (products.Count > 0)
                {
                    var productview = _mapper.Map<IEnumerable<ProductViewDTO>>(products);
                    return productview;
                }
                return new List<ProductViewDTO>();
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<ProductViewDTO>>Search(string name)
        {
            try
            {
               
                var products = await _context.Products.Include(p => p.Category).Where(pr => pr.Title.ToLower().Contains(name.ToLower()) && pr.status==true).ToListAsync();
                if (products.Count == 0)
                {
                    return new List<ProductViewDTO>();
                }
                var product_cl = _mapper.Map<IEnumerable<ProductViewDTO>>(products);
                return product_cl;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddProduct(ProductDTO product)
        {
            try
            {
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
                if (!categoryExists)
                {
                    _logger.LogWarning("Category with ID {CategoryId} does not exist.", product.CategoryId);
                    return false; 
                }

                var prod = _mapper.Map<Products>(product);
                await _context.Products.AddAsync(prod);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateProduct(int id, ProductDTO product)
        {
            try
            {
                var exist = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (exist == null)
                {
                    return false;
                }
                exist.Title = product.Title;
                exist.Description = product.Description;
                exist.Price = product.Price;
                exist.CategoryId = product.CategoryId;
                exist.Img = product.Img;
                exist.Quantity = product.Quantity;
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var exist = await _context.Products.FirstOrDefaultAsync(pr => pr.Id == id);
                if (exist == null)
                {
                    return false;
                }
                exist.status = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
       


    }
}
