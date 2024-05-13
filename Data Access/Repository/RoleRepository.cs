using InterviewPuzzle.Data_Access.Model;
using Microsoft.AspNetCore.Identity;

namespace InterviewPuzzle.Data_Access.Repository
{
    public class RoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                return Enumerable.Empty<string>();
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<IdentityResult> CreateRoleAsync(string role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            return result;
        }

        public async Task<IdentityResult> AssignRoleAsync(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                var error = new IdentityError { 
                    Code = "UserNotFound", 
                    Description = $"User with username '{username}' does not exist." 
                };
                return IdentityResult.Failed(error);

            }
            var result = await _userManager.AddToRoleAsync(user, role);
            return result;
        } 
    }
}
