using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var token = await _accountRepository.LoginAsync(login);
            if(token == null)
            {
                return Unauthorized();
            }
            return Ok(new {Token = token});
        }

        [HttpPost("register")] 
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            var result = await _accountRepository.CreatUserAsync(register);
            return Ok(result);
        }



    }
}
