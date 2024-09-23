using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs;
using Ecommerce_Webapi.Models.UserModel;
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
                if (User == null)
                {
                    return NotFound("Users not found");

                }
                return Ok(users);


            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server Error:{ex} occured");
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
                    return NotFound("User not found");
                }
                return Ok(users);
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :ex.Message");
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult>SignIn(UserDTO userDTO)
        {
            try
            {
                var val = await _userService.Register_User(userDTO);
                if (val==false)
                {
                    return BadRequest("Please Login");
                }
                return Ok("Sucessfully Registered");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :ex.Message");
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
                    return BadRequest("Please SignUp");
                }
                if(response== "Wrong Password")
                {
                    return BadRequest("Wrong Password");
                }
                if(response=="User Blocked")
                {
                    return StatusCode(404, "Forbidden");
                }
                return Ok(new { Token = response });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
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
                    return BadRequest("User not found");
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :ex.Message");
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
                    return BadRequest("User not found");
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :ex.Message");
            }
        }

    }
}
