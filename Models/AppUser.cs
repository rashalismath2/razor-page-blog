
using Microsoft.AspNetCore.Identity;

namespace BlogSite.Models;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string? ProfileUrl { get; set; }
    public string FullName { get; set; }
    public List<GalleryImages>? GalleryImages { get; set; }
    public List<Post> Articles { get; set; }
}

