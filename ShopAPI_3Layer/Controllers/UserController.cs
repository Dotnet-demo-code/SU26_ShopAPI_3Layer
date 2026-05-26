using BusinessLayer.IServices;
using DataAccessLayer.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI_3Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var result = _userService.LoginAsync(userLoginDTO).Result;
            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var createdUser = _userService.RegisterAsync(userRegisterDTO).Result;
                return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllAsync().Result;
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetByIdAsync(id).Result;
            if (user == null)
                return NotFound(new { message = "User không tồn tại" });

            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, [FromBody] UserDTO userUpdateDTO)
        {
            if (id != userUpdateDTO.UserId)
                return BadRequest(new { message = "ID không khớp" });

            try
            {
                var updatedUser = _userService.UpdateAsync(userUpdateDTO).Result;
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var result = _userService.DeleteAsync(id).Result;
            if (!result)
                return NotFound(new { message = "User không tồn tại" });

            return Ok(new { message = "Xoá user thành công" });
        }
    }
}
