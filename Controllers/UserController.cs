using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.Data.UserDtOs;
using Ecommerce_Webapi.Models.UserModel;
using Ecommerce_Webapi.Responses;
using Ecommerce_Webapi.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("All")]
        public async Task<IActionResult> Get_All_User()
        {
            _logger.LogInformation("fetching all users");
            try
            {
                var users = await _userService.GetAll();
                var response = new ApiResponse<IEnumerable<OutUsers>> (200, "User fetched Successfully", users);
                if (User == null)
                {
                    return NotFound(new ApiResponse<IEnumerable<OutUsers>>(404, "No user found", null));

                }
                return Ok(response);


            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>(500, "Internal server issue", null, ex.Message);
                return StatusCode(500,response);
            }
        }
        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Get_by_Id(int id)
        {
            try
            {
                var users = await _userService.GetById(id);
                if (users == null)
                {
                    return NotFound(new ApiResponse<OutUsers>(404, "No user found", null));
                }
                var response = new ApiResponse<OutUsers>(200, "User successfully fetched", users);
                return Ok(response);
                
            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>(500, "Internal server issue", null, ex.Message);
                return StatusCode(500, response);
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult>SignIn(UserDTO userDTO)
        {
            try
            {
                var val = await _userService.Register_User(userDTO);
                var response = new ApiResponse<bool>(200, "Sucessfully registered");
                if (val==false)
                {
                    return BadRequest(new ApiResponse<bool>(400, "Email already Exist"));
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>(500, "Internal server issue", null, ex.Message);
                return StatusCode(500, response);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult>Login_(Login login)
        {
            try
            {
                var response = await _userService.Login(login);
                if (response== "Not Found")
                {
                    return NotFound(new ApiResponse<string>(404,"Please SignUp, user not found"));
                }
                if(response== "Wrong Password")
                {
                    return BadRequest(new ApiResponse<string>(400, "Wrong Password"));
                }
                if(response=="User Blocked")
                {
                    return StatusCode(404,new ApiResponse<string>(404,"User is blocked by admin"));
                }
                return Ok(new ApiResponse<string>(200, "Login successfully", response));
                
            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>(500, "Internal server issue", null, ex.Message);
                return StatusCode(500, response);
            }        
        }
        [HttpPut("block/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>Block(int id)
        {
            try
            {
                var resp = await _userService.Block_User(id);
                if (resp == false)
                {
                    return NotFound(new ApiResponse<string>(404, "User not found"));
                }
                return Ok(new ApiResponse<string>(200 ,$"User with id {id} is blocked successfuly"));
            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>(500, "Internal server issue", null, ex.Message);
                return StatusCode(500, response);
            }
        }
        [HttpPut("Unblock/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>Unblock(int id)
        {
            try
            {
                var rsp = await _userService.Unblock_User(id);
                if(rsp== false)
                {
                    return NotFound(new ApiResponse<string>(404, "User not found"));
                }
                return Ok(new ApiResponse<string>(200, $"user with id {id} successfully unblocked"));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal server issue", null, ex.Message));
            }
        }
        [HttpDelete("Delete{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                var res = await _userService.DeleteUser(id);
                if(res == false)
                {
                    return NotFound(new ApiResponse<string>(404, "User not found"));
                }
                return Ok(new ApiResponse<string>(200, "Successfully deleted"));
            }
            catch(Exception ex)
            {
                return StatusCode(500,new ApiResponse<string>(500, "Internal server issue", null, ex.Message));
            }
        }

    }
}
