﻿
using Microsoft.AspNetCore.Identity;

namespace BlogSite.Models;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string? ProfileUrl { get; set; }
}

