using InterviewPuzzle.Data_Access;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPuzzle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RoleManagementController : ControllerBase
    {
        private readonly RoleRepository _roleRepository;
        private readonly IUnitOfWork _uow;

        public RoleManagementController(RoleRepository roleRepository, IUnitOfWork uow)
        {
            _roleRepository = roleRepository;
            _uow = uow;
        }

        [HttpGet("{username}/roles")]
        public async Task<IActionResult> GetUserRoles(string username)
        {
            var roles = await _roleRepository.GetUserRolesAsync(username);
            return Ok(roles);
        }

        [HttpPost("create-role/{name}")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var result = await _roleRepository.CreateRoleAsync(name.ToLower());
            return Ok(result);
        }
        [HttpPost("assign/{username}/to/{role}")]
        public async Task<IActionResult> AssignRole(string username, string role)
        {
            var result = await _roleRepository.AssignRoleAsync(username, role);
            return Ok(result);
        }
    }
}
