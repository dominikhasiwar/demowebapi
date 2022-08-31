using DemoWebApi.Managers;
using DemoWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]    
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<UserModel[]> GetUsers()
        {
            return await _userManager.GetUsers();
        }

        [HttpGet("{userId}")]
        [MapToApiVersion("1.0")]
        public async Task<UserModel> GetUser(int userId)
        {
            return await _userManager.GetUser(userId);
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<UserModel> CreateUser([FromBody] SaveUserModel model)
        {
            return await _userManager.CreateUser(model);
        }

        [HttpPut("{userId}")]
        [MapToApiVersion("1.0")]
        public async Task<UserModel> UpdateUser(int userId, [FromBody] SaveUserModel model)
        {
            return await _userManager.UpdateUser(userId, model);
        }

        [HttpDelete("{userId}")]
        [MapToApiVersion("2.0")]
        public void DeleteUser(int userId)
        {
            _userManager.DeleteUser(userId);
        }
    }
}
