using AutoMapper;
using BCrypt.Net;
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs;
//using Ecommerce_Webapi.Models.DTO;
using Ecommerce_Webapi.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Ecommerce_Webapi.Services.UserService
{
    public class UserServices :IUserService
    {
        private AppDbContext _context;
        private IMapper _mapper;
        private IConfiguration _configuration;
        
        public UserServices(AppDbContext context,IMapper mapper,IConfiguration configuration) 
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            
        }
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            try
            {

                var users = await _context.Users.ToListAsync();
                var userdto = _mapper.Map<IEnumerable<UserDTO>>(users);
                return userdto;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<UserDTO> GetById(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(us => us.Id == id);
                if (user == null)
                {
                    return null;
                }
                var userdto = _mapper.Map<UserDTO>(user);
                return userdto;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
       public async Task<bool> Register_User(UserDTO userDTO)
        {
            try
            {
                var exist = await _context.Users.FirstOrDefaultAsync(user => user.UserEmail == userDTO.User_Email);
                if (exist == null)
                {
                    var Hashpass = BCrypt.Net.BCrypt.EnhancedHashPassword(userDTO.User_Password, HashType.SHA256);
                    var newuser = new Users() { UserName = userDTO.User_Name, UserEmail = userDTO.User_Email, Password = Hashpass };
                    await _context.Users.AddAsync(newuser);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           

        }
        public async Task<string> Login(Login login)
        {
            try
            {
                var exist = await _context.Users.FirstOrDefaultAsync(us => us.UserEmail == login.Email);
                if (exist !=null)
                {
                    var password = BCrypt.Net.BCrypt.EnhancedVerify(login.Password, exist.Password);
                    if (password)
                    {
                        if (exist.IsStatus == false)
                        {
                            return "User Blocked";
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var claim = new[]
             {
                //new Claim(JwtRegisteredClaimNames.Sub,user.Name),
                //new Claim(JwtRegisteredClaimNames.Jti,user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier,exist.Id.ToString()),
                new Claim(ClaimTypes.Role,exist.Role)
            };
                        var token = new JwtSecurityToken(
                claims: claim,
                signingCredentials: credential,
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],

                expires: DateTime.Now.AddDays(1));
                        return new JwtSecurityTokenHandler().WriteToken(token);
                    }

                }
                return "Not Found";
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Block_User(int id)
        {
            try
            {
                var exist = await _context.Users.FirstOrDefaultAsync(us => us.Id == id);
                if (exist == null)
                {
                    return false;
                }
                exist.IsStatus = false;
                await _context.SaveChangesAsync();
                return true;
               

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<bool> Unblock_User(int id)
        {
            try
            {
                var exist = await _context.Users.FirstOrDefaultAsync(us => us.Id == id);
                if (exist == null)
                {
                    return false;
                }
                exist.IsStatus = true;
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
