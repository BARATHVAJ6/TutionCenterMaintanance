using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Services;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Auth;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName)
                   ?? await _userManager.FindByEmailAsync(request.UserName);

        if (user == null || !user.IsActive)
        {
            return Ok(ApiResponse<LoginResponse>.Fail("Invalid username or password."));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Ok(ApiResponse<LoginResponse>.Fail("Invalid username or password."));
        }

        var response = await _tokenService.GenerateTokenAsync(user);
        return Ok(ApiResponse<LoginResponse>.Ok(response, "Login successful"));
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Ok(ApiResponse<bool>.Fail("Failed to change password", result.Errors.Select(e => e.Description).ToList()));
        }

        return Ok(ApiResponse<bool>.Ok(true, "Password changed successfully"));
    }

    [HttpGet("users")]
    [Authorize(Policy = Permissions.UsersView)]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var dtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "SubAdmin";
            var permissions = await _context.RolePermissions
                .Where(rp => rp.Role.Name != null && roles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission)
                .Distinct()
                .ToListAsync();

            dtos.Add(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                RoleName = roleName,
                Permissions = permissions
            });
        }

        return Ok(ApiResponse<List<UserDto>>.Ok(dtos));
    }

    [HttpPost("users")]
    [Authorize(Policy = Permissions.UsersManage)]
    public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserRequest request)
    {
        var existing = await _userManager.FindByNameAsync(request.UserName);
        if (existing != null)
        {
            return Ok(ApiResponse<UserDto>.Fail("Username is already taken."));
        }

        var user = new AppUser
        {
            UserName = request.UserName,
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = true,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Ok(ApiResponse<UserDto>.Fail("Failed to create user", result.Errors.Select(e => e.Description).ToList()));
        }

        if (await _roleManager.RoleExistsAsync(request.RoleName))
        {
            await _userManager.AddToRoleAsync(user, request.RoleName);
        }

        var dto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            RoleName = request.RoleName
        };

        return Ok(ApiResponse<UserDto>.Ok(dto, "User created successfully"));
    }

    [HttpPut("users/{id}")]
    [Authorize(Policy = Permissions.UsersManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateUser(string id, [FromBody] UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.IsActive = request.IsActive;

        await _userManager.UpdateAsync(user);

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (await _roleManager.RoleExistsAsync(request.RoleName))
        {
            await _userManager.AddToRoleAsync(user, request.RoleName);
        }

        return Ok(ApiResponse<bool>.Ok(true, "User updated successfully"));
    }

    [HttpGet("roles")]
    [Authorize(Policy = Permissions.RolesManage)]
    public async Task<ActionResult<ApiResponse<List<RoleDto>>>> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var dtos = new List<RoleDto>();

        foreach (var r in roles)
        {
            var perms = await _context.RolePermissions
                .Where(rp => rp.RoleId == r.Id)
                .Select(rp => rp.Permission)
                .ToListAsync();

            dtos.Add(new RoleDto
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty,
                Description = r.Description,
                Permissions = perms
            });
        }

        return Ok(ApiResponse<List<RoleDto>>.Ok(dtos));
    }

    [HttpPost("roles")]
    [Authorize(Policy = Permissions.RolesManage)]
    public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole([FromBody] CreateOrUpdateRoleRequest request)
    {
        if (await _roleManager.RoleExistsAsync(request.Name))
        {
            return Ok(ApiResponse<RoleDto>.Fail("Role name already exists."));
        }

        var role = new AppRole
        {
            Name = request.Name,
            Description = request.Description
        };

        await _roleManager.CreateAsync(role);

        foreach (var p in request.Permissions)
        {
            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                Permission = p
            });
        }
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<RoleDto>.Ok(new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = request.Permissions
        }, "Role created successfully"));
    }

    [HttpPut("roles/{id}")]
    [Authorize(Policy = Permissions.RolesManage)]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateRole(string id, [FromBody] CreateOrUpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();

        role.Description = request.Description;
        await _roleManager.UpdateAsync(role);

        // Update permissions
        var existing = await _context.RolePermissions.Where(rp => rp.RoleId == id).ToListAsync();
        _context.RolePermissions.RemoveRange(existing);

        foreach (var p in request.Permissions)
        {
            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = id,
                Permission = p
            });
        }
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Role permissions updated successfully"));
    }

    [HttpGet("permissions")]
    [Authorize]
    public ActionResult<ApiResponse<IReadOnlyList<string>>> GetAllPermissions()
    {
        return Ok(ApiResponse<IReadOnlyList<string>>.Ok(Permissions.GetAll()));
    }
}
