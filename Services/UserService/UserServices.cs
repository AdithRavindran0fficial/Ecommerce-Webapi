using AutoMapper;
using BCrypt.Net;
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs;
//using Ecommerce_Webapi.Models.DTO;
using Ecommerce_Webapi.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce_Webapi.Services.UserService
{
    public class UserServices
    {
        private AppDbCotext _context;
        private IMapper _mapper;
        private IConfiguration _configuration;
        public UserServices(AppDbCotext context,IMapper mapper,IConfiguration configuration) 
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
            var user = await _context.Users.FirstOrDefaultAsync(us => us.Id==id);
            var userdto = _mapper.Map<UserDTO>(user);
            return userdto;
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
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = _configuration["Jwt:Key"];
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.NameIdentifier,exist.Id.ToString()),
                            new Claim(ClaimTypes.Name,exist.UserName),
                            new Claim(ClaimTypes.Role,exist.Role),
                            }),
                            Expires = DateTime.UtcNow.AddMinutes(30),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);
                        return tokenString;


                    }

                }
                return "Not Found";
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
