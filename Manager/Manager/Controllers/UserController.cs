using Manager.Application.Astraction.Services;
using Manager.Doman.Entites;
using Manager.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserController : Controller
    {
        //ToDo add ExecutionResponse and implement the logic
        private readonly IUserService _userService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtTokenService _jwtTokenService;
        public UserController(
            IUserService userService, 
            IPasswordHasherService passwordHasherService,
            IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _passwordHasherService = passwordHasherService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AddUserRequstDTO request)
        {
            var hashedPassword = _passwordHasherService.HashPassword(request.Password);
            var user = new User
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = hashedPassword
            };
            var result = await _userService.CreateUserAsync(user);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequestDTO request)
        {
            var userResult = await _userService.FindUserAsync(u => u.UserName == request.UserName);
            if (!userResult.Success) 
                return Unauthorized();//
            
            var user = (User)userResult.Result;
            if (!_passwordHasherService.VerifyPassword(user.PasswordHash ,request.Password))
                return Unauthorized();

            var token = _jwtTokenService.GenerateToken(user);
            
            return Ok(token);
        }
    }
}
