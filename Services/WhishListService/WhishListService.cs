using AutoMapper;
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs.WhishListDTO;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Services.JWTServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webapi.Services.WhishListService
{
    public class WhishListService : IWhishList
    {
        private IJWTServices _jwt;
        private AppDbContext _context;
        private IMapper _mapper;
        private ILogger<WhishListService> _logger;
        private IConfiguration _configuration;
        public WhishListService(IJWTServices jwt,AppDbContext context,IMapper mapper,ILogger<WhishListService>logger,IConfiguration configuration) 
        {
            _jwt = jwt;
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> AddToWhishList(string token, int productid)
        {
            try
            {
                var userid = _jwt.GetUserId(token);
                var user = await _context.Users.Include(wh => wh.WhishList)
                            .ThenInclude(pr => pr.Products).FirstOrDefaultAsync(us => us.Id == userid);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var product = user.WhishList.FirstOrDefault(wh => wh.ProductId == productid);
                if (product == null)
                {
                    AddWhishListDTO item = new AddWhishListDTO
                    {
                        ProductId = productid,
                        UserId = userid
                    };
                    var map = _mapper.Map<WhishList>(item);
                    _context.WhishList.Add(map);
                    await _context.SaveChangesAsync();
                    return true;

                }
                
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable< OutWhishList>> GetItems(string token)
        {
            try
            {
                var userid = _jwt.GetUserId(token);
                var user = await _context.Users.Include(wh => wh.WhishList)
                    .ThenInclude(pr => pr.Products).ThenInclude(ct=>ct.Category)
                    .FirstOrDefaultAsync(us => us.Id == userid);
                if(user == null)
                {
                    throw new Exception("User not found");
                   
                }
                if (user.WhishList != null && user.WhishList.Any())
                {
                    var items = user.WhishList.Select(itm => new OutWhishList
                    {
                        Id = itm.Id,
                        ProductId = itm.ProductId,
                        Title = itm.Products.Title,
                        Description = itm.Products.Description,
                        Img = $"{_configuration["HostUrl:images"]}/Products/{itm.Products.Img}",
                        price = itm.Products.Price,
                        category = itm.Products.Category.CategoryName,

                    }).ToList();
                    return items;
                }
                return new List<OutWhishList>();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> RemoveWhishlist(string token, int productid)
        {
            try
            {
                var userid = _jwt.GetUserId(token);
                var user = await _context.Users.Include(wh=>wh.WhishList)
                    .ThenInclude(pr=>pr.Products).FirstOrDefaultAsync(us=>us.Id==userid);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var item = user.WhishList.FirstOrDefault(pr => pr.ProductId == productid);
                if (item == null)
                {
                    return false;
                }
                user.WhishList.Remove(item);
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
