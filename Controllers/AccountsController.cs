using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterviewPuzzle.Controllers
{
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
        /// log in user
        /// </summary>
        /// <param name="login"></param>
        /// <returns>An API response for login attempt</returns>
        /// <response code="200">Returns a jwt token</response>
        /// <response code="401">If the login failed due to wrong username or password</response>
 
        [HttpPost("login")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var token = await _accountRepository.LoginAsync(login);
            var response = new APIResponse<string>();
            if(token == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.ErrorMessages.Add("wrong username or password");
                return Unauthorized(response);
            }

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = token;
            return Ok(response);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="register">The registration details.</param>
        /// <returns>An API response with the registration result.</returns>
        /// <response code="201">Returns the newly created user details</response>
        /// <response code="401">If the registration failed due to unauthorized access</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(APIResponse<IdentityResult>), 201)]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            var result = await _accountRepository.CreatUserAsync(register);
            var response = new APIResponse<IdentityResult>();
            if (result.Succeeded == false)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.Result = result;   
                return Unauthorized(response);
            }
            response.IsSuccess= true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = result;
            return Ok(response);
        }

    }
}
