using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotApi.Models;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("xiaozhi/admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: xiaozhi/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<SysUser>>> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string mobile = null)
        {
            var query = _context.SysUsers.AsQueryable();

            if (!string.IsNullOrEmpty(mobile))
            {
                // Note: In the original Java code, this filters by mobile, but our SysUser model doesn't have a mobile field
                // We'll filter by username instead as a placeholder
                query = query.Where(u => u.Username.Contains(mobile));
            }

            var users = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // In a real implementation, we would return pagination info
            return Ok(users);
        }

        // PUT: xiaozhi/admin/users/{id}
        [HttpPut("users/{id}")]
        public async Task<ActionResult<string>> ResetPassword(long id)
        {
            var user = await _context.SysUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Generate a new random password (in a real app, this would be more secure)
            string newPassword = GenerateRandomPassword();
            user.Password = newPassword; // In a real app, this should be hashed
            user.UpdateDate = DateTime.UtcNow;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(newPassword);
        }

        // DELETE: xiaozhi/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.SysUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.SysUsers.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: xiaozhi/admin/users/changeStatus/{status}
        [HttpPut("users/changeStatus/{status}")]
        public async Task<IActionResult> ChangeStatus(int status, [FromBody] long[] userIds)
        {
            var users = await _context.SysUsers
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            foreach (var user in users)
            {
                user.Status = status;
                user.UpdateDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: xiaozhi/admin/device/all
        [HttpGet("device/all")]
        public async Task<ActionResult<IEnumerable<object>>> GetDevices(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string keywords = null)
        {
            // In the Java code, this returns device information
            // Since we don't have a complete device model implementation yet, we'll return an empty list
            var devices = new List<object>();
            return Ok(devices);
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}