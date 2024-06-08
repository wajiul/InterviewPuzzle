using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterviewPuzzle.Controllers
{
    /// <summary>
    /// Controller for managing account operations such as login and registration.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountRepository _accountRepository;

        public AccountsController(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="login">The login details.</param>
        /// <returns>An API response with a JWT token on success.</returns>
        /// <response code="200">Returns a JWT token on successful login.</response>
        /// <response code="401">If the login fails due to incorrect username or password.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var token = await _accountRepository.LoginAsync(login);

            if (token == null)
            {
                return Unauthorized(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorMessages = new List<string>() { "Incorrect username or password." }
                });
            }

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = token
            });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="register">The registration details.</param>
        /// <returns>An API response with the registration result.</returns>
        /// <response code="200">Returns success message of successful registration.</response>
        /// <response code="400">If the registration fails due to invalid data.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            var result = await _accountRepository.CreateUserAsync(register);

            if (!result.Succeeded)
            {
                return BadRequest( new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList()
                });
            }

            return Ok(new APIResponse<string> {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = "Registration succesful."
            });
        }
    }
}
