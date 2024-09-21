using Ecommerce_Webapi.Controllers.Models.User;
using Ecommerce_Webapi.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webapi.Data
{
    public class AppDbCotext : DbContext
    {
        public AppDbCotext(DbContextOptions option)
            :base(option)
        {

        }
        public DbSet<Users> Users { get; set; }
    }
}
