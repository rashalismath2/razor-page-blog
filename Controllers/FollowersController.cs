
using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    [Route("api/followers")]
    [ApiController]
    [Authorize]
    public class FollowersController : Controller
    {
        private readonly AppDbContext _dbContext;

        public FollowersController(AppDbContext dbContex)
        {
            _dbContext = dbContex;
        }
        [HttpGet("is-following")]
        public async Task<ActionResult<bool>> IsFollowing([FromQuery] string targetUserId)
        {
            var userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;

            var isFollowing = await _dbContext.AppUsers.Where(u => u.Id == targetUserId)
                .Select(u => u.Followers != null && u.Followers.Any(f => f.Id == userId)).FirstOrDefaultAsync();

            return Ok(isFollowing);
        }
        [HttpGet("follow")]
        public async Task<ActionResult<bool>> Follow([FromQuery] string targetUserId)
        {
            var userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;
            var taretUser = await _dbContext.AppUsers.Where(u => u.Id == targetUserId).FirstOrDefaultAsync();
            var loggedInUser = await _dbContext.AppUsers.Where(u => u.Id == userId).FirstOrDefaultAsync();
            taretUser.Followers = new List<AppUser> { loggedInUser };

            _dbContext.Update(taretUser);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("un-follow")]
        public async Task<ActionResult<bool>> UnFollow([FromQuery] string targetUserId)
        {
            var userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;
            var taregtUser = await _dbContext.AppUsers.Include(f => f.Followers.Where(u => u.Id == userId))
                .Where(u => u.Id == targetUserId)
                .FirstOrDefaultAsync();

            var followingUser = taregtUser.Followers.FirstOrDefault();
            if (followingUser is null)
            {
                return BadRequest();
            }
            taregtUser.Followers.Remove(followingUser);
            _dbContext.Update(taregtUser);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
