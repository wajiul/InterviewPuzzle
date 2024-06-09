using InterviewPuzzle.Data_Access;
using InterviewPuzzle.Data_Access.Model;
using InterviewPuzzle.Data_Access.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace InterviewPuzzle.Controllers
{
    /// <summary>
    /// Controller for managing user roles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Authorize]
    public class RoleManagementController : ControllerBase
    {
        private readonly RoleRepository _roleRepository;
        private readonly IUnitOfWork _uow;

        public RoleManagementController(RoleRepository roleRepository, IUnitOfWork uow)
        {
            _roleRepository = roleRepository;
            _uow = uow;
        }

        /// <summary>
        /// Gets the roles assigned to a specific user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A list of roles assigned to the user.</returns>
        /// <response code="200">Returns the list of roles assigned to the user.</response>
        /// <response code="404">If the user is not found.</response>
        /// <remarks>
        /// This endpoint requires authentication.
        /// </remarks>
        [HttpGet("{username}/roles")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<string>>), 200)]
        public async Task<IActionResult> GetUserRoles(string username)
        {
            var roles = await _roleRepository.GetUserRolesAsync(username);
            if (roles == null)
            {
                return NotFound(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessages = new List<string> { $"User '{username}' not found" }
                });
            }

            return Ok(new APIResponse<IEnumerable<string>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = roles
            });
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="name">The name of the role to create.</param>
        /// <returns>The result of the role creation.</returns>
        /// <response code="201">Returns the created role.</response>
        /// <response code="400">If the role creation fails.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpPost("create-role/{name}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(APIResponse<string>), 201)]
        public async Task<IActionResult> CreateRole(string name)
        {
            var result = await _roleRepository.CreateRoleAsync(name.ToLower());
            if (!result.Succeeded)
            {
                return BadRequest(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList()
                });
            }

            return CreatedAtAction(nameof(GetUserRoles), new { username = name }, new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = "role created sucessfully."
            });
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="role">The role to assign.</param>
        /// <returns>The result of the role assignment.</returns>
        /// <response code="200">Returns the result of the role assignment.</response>
        /// <response code="400">If the role assignment fails.</response>
        /// <remarks>
        /// This endpoint requires authentication with the admin role.
        /// </remarks>
        [HttpPost("assign/{username}/to/{role}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(APIResponse<string>), 200)]
        public async Task<IActionResult> AssignRole(string username, string role)
        {
            var result = await _roleRepository.AssignRoleAsync(username, role);
            if (!result.Succeeded)
            {
                return BadRequest(new APIResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList()
                });
            }

            return Ok(new APIResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = $"Assigned user: {username} to role: {role}"
            });
        }
    }
}
