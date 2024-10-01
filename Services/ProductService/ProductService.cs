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
        private ILogger<ProductService> _logger;
        private IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _configuration;
        public ProductService(AppDbContext context,IMapper mapper,ILogger<ProductService>logger,IWebHostEnvironment webhostenvironment,IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webhostenvironment;
            _configuration = configuration;
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
                        Img = $"{_configuration["HostUrl:images"]}/Products/{p.Img}" ,
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
                product.Img = $"{_configuration["HostUrl:images"]}/Products/{product.Img}";
                return product;
                

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
           
        }
        public IEnumerable<ProductViewDTO> GetProductByCat(CategoryDTO category)
        {
            try
            {
                var products =  _context.Products.Include(p => p.Category).Where(p => p.Category.CategoryName == category.CategoryName && p.status==true);
                var productcl = products.Select(pr => new ProductViewDTO
                {
                    Id = pr.Id,
                    Title = pr.Title,
                    Description = pr.Description,
                    Img = $"{_configuration["HostUrl:images"]}/Products/{pr.Img}",
                    Price = pr.Price,
                    Category = pr.Category.CategoryName,
                    Quantity = pr.Quantity
                }).ToList();
                if (productcl.Count > 0)
                {
                    var productview = _mapper.Map<IEnumerable<ProductViewDTO>>(productcl);
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
        public IEnumerable<ProductViewDTO>Search(string name)
        {
            try
            {
               
                var products =  _context.Products.Include(p => p.Category).Where(pr => pr.Title.ToLower().Contains(name.ToLower()) && pr.status==true);
                var productcl = products.Select(pr => new ProductViewDTO
                {
                    Id = pr.Id,
                    Title = pr.Title,
                    Description = pr.Description,
                    Img = $"{_configuration["HostUrl:images"]}/Products/{pr.Img}",
                    Quantity = pr.Quantity,
                    Category = pr.Category.CategoryName,
                    Price = pr.Price
                }).ToList();
                if (productcl.Count == 0)
                {
                    return new List<ProductViewDTO>();
                }
                var product_cl = _mapper.Map<IEnumerable<ProductViewDTO>>(productcl);
                return product_cl;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddProduct(Addproduct product,IFormFile img)
        {
            try
            {
                var exist = await _context.Products.FirstOrDefaultAsync(pr => pr.Title == product.Title);
                if(exist != null)
                {
                    return false;
                }
                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
                if (!categoryExists)
                {
                    _logger.LogWarning("Category with ID {CategoryId} does not exist.", product.CategoryId);
                    return false; 
                }
                
                var prod = _mapper.Map<Products>(product);
                if (img != null&& img.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images","Products", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    
                    prod.Img = fileName;
                }

               
                
                await _context.Products.AddAsync(prod);
               var changes=  await _context.SaveChangesAsync();
                _logger.LogInformation($"{changes} ocured");
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateProduct(int id, Addproduct product,IFormFile img)
        {
            try
            {
                var exist = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (exist == null)
                {
                    return false;
                }
                string productimg = null;
                if(img!=null  && img.Length > 0)
                  {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.Name);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath,"Images","Products",fileName);
                    using(var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                        productimg = fileName; 
                    }

                }
                
                exist.Title = product.Title;
                exist.Description = product.Description;
                exist.Price = product.Price;
                exist.CategoryId = product.CategoryId;
                exist.Img = productimg;
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
