using BlogSite.Models;
using BlogSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    [Route("api/social")]
    [ApiController]
    [Authorize]
    public class SocialInteractionController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public SocialInteractionController(AppDbContext dbContex)
        {
            _dbContext = dbContex;
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<SocialInteractionVm>> Social([FromRoute] int id)
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            string userId = null;
            if (isAuthenticated)
            {
                userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;
            }


            var socials = await _dbContext.Posts.Where(p => p.Id == id).Include(p => p.Likes).Include(p => p.Comments).ThenInclude(c => c.CommentedUser)
                .Select(p => new SocialInteractionVm
                {
                    Likes = p.Likes,
                    Comments = p.Comments,
                    IsLiked = isAuthenticated ? p.Likes.FirstOrDefault(l => l.LikedUser.Id == userId) != null : false,
                    IsSocialAllowed = isAuthenticated
                })
                .FirstOrDefaultAsync();


            return Ok(socials);
        }
        [HttpPost("comment")]
        public async Task<ActionResult<SocialInteractionVm>> Comments(AddCommentRequestVm comment)
        {
            string userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var post = await _dbContext.Posts.Where(p => p.Id == comment.PostId)
                .FirstOrDefaultAsync();
            var newComment = new Comment
            {
                Comments = comment.Comment,
                CommentedUser = user,
                Date = DateTime.Now
            };
            post.Comments = new List<Comment> {
               newComment
            };

            _dbContext.Update(post);
            await _dbContext.SaveChangesAsync();

            newComment.CommentedUser.Articles = null;

            return Ok(newComment);
        }
        [HttpPost("{id}/unlike")]
        public async Task<ActionResult<SocialInteractionVm>> UnLike([FromRoute] int id)
        {
            string userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;

            var post = await _dbContext.Posts.Where(p => p.Id == id)
                .Include(l => l.Likes.Where(l => l.LikedUser.Id == userId))
                .FirstOrDefaultAsync();

            var like = post.Likes.FirstOrDefault();
            post.Likes.Remove(like);
            _dbContext.Update(post);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("{id}/like")]
        public async Task<ActionResult<SocialInteractionVm>> Like([FromRoute] int id)
        {
            string userId = User.Claims.ToList().FirstOrDefault((c) => c.Type == "Id").Value;
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var post = await _dbContext.Posts.Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            post.Likes = new List<Like> {
                new Like{
                    LikedUser=user
                }
            };

            _dbContext.Update(post);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
