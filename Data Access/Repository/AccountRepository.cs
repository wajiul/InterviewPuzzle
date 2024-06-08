using InterviewPuzzle.Data_Access.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InterviewPuzzle.Data_Access.Repository
{
    public class AccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(Login login)
        {
            var result = await _signInManager.PasswordSignInAsync(
                          login.Username, login.Password, false, false
                        );

            if (!result.Succeeded)
            {
                return null;
            }
            var user = await _userManager.FindByNameAsync(login.Username);
            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user.UserName, user.FirstName, user.LastName, roles);
            return token;
        }

        private string GenerateJwtToken(string username, string firstname, string lastname, IList<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("username", username),
            new Claim("firstname", firstname),
            new Claim("lastname", lastname)
            };

            foreach (var role in roles)
               claims.Add(new Claim("roles", role));


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> CreateUserAsync(Register register)
        {
            var existingEmail = await _userManager.FindByEmailAsync(register.Email); 

            if (existingEmail != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email is already registered." });
            }

            var newUser = new ApplicationUser()
            {
                Email = register.Email,
                UserName = register.Username
            };

            var result = await _userManager.CreateAsync(newUser, register.Password);
            return result;
        }

    }
}
