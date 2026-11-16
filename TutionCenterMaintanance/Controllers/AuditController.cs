using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TutionCenterMaintanance.Data;
using TutionCenterMaintanance.Data.Entities;
using TutionCenterMaintanance.Shared.Constants;
using TutionCenterMaintanance.Shared.DTOs.Audit;
using TutionCenterMaintanance.Shared.Models;

namespace TutionCenterMaintanance.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuditController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize(Policy = Permissions.AuditView)]
    public async Task<ActionResult<ApiResponse<List<AuditLogDto>>>> GetAuditLogs(
        [FromQuery] string? entityName = null,
        [FromQuery] string? actionType = null)
    {
        var query = _context.AuditLogs.AsQueryable();

        if (!string.IsNullOrEmpty(entityName))
            query = query.Where(a => a.EntityName == entityName);

        if (!string.IsNullOrEmpty(actionType))
            query = query.Where(a => a.Action == actionType);

        var users = await _context.Users.ToDictionaryAsync(u => u.Id, u => u.FullName);

        var logs = await query
            .OrderByDescending(a => a.Timestamp)
            .Take(150)
            .ToListAsync();

        var dtos = logs.Select(a => new AuditLogDto
        {
            Id = a.Id,
            EntityName = a.EntityName,
            EntityId = a.EntityId,
            Action = a.Action,
            OldValues = a.OldValues,
            NewValues = a.NewValues,
            UserId = a.UserId,
            UserName = !string.IsNullOrEmpty(a.UserId) && users.ContainsKey(a.UserId) ? users[a.UserId] : "Administrator",
            Timestamp = a.Timestamp
        }).ToList();

        return Ok(ApiResponse<List<AuditLogDto>>.Ok(dtos));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<bool>>> RecordAuditLog([FromBody] AuditLogDto dto)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var log = new AuditLog
        {
            EntityName = dto.EntityName,
            EntityId = dto.EntityId,
            Action = dto.Action,
            OldValues = dto.OldValues,
            NewValues = dto.NewValues,
            UserId = currentUserId,
            Timestamp = DateTime.UtcNow
        };

        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Ok(true, "Audit event recorded"));
    }
}
