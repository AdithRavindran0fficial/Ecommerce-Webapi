using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.Models.DTO;

namespace Ecommerce_Webapi.Services.UserService
{
    public class UserServices
    {
        private AppDbCotext _context;
        public UserServices(AppDbCotext context) 
        {
            _context = context;
        }
        public Task<IEnumerable<UserDTO>> GetAll()
        {
            return _context.
        }
    }
}
