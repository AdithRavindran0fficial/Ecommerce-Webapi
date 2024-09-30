using Ecommerce_Webapi.Data.UserDtOs;
using Ecommerce_Webapi.Models.UserModel;

namespace Ecommerce_Webapi.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<OutUsers>> GetAll();
        Task<OutUsers> GetById(int id);
        Task<bool> Register_User(UserDTO userDTO);
        //Task<UserDTO> Update_User(int id, UserDTO userDTO);
        Task<string> Login(Login login);
        Task<bool> Block_User(int id);
        Task<bool> Unblock_User(int id);
        Task<bool> DeleteUser(int id);

    }
}
