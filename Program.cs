using BlogSite.Repository;
using Microsoft.EntityFrameworkCore;
using BlogSite.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseMySql(builder.Configuration.GetConnectionString("MySQl"), serverVersion));

builder.Services
    .AddDefaultIdentity<AppUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(65);

    options.LoginPath = "/authentication/Login";
    options.LogoutPath = "/authentication/logout";
});



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;
app.UseAuthorization();

app.MapRazorPages();

app.Run();
