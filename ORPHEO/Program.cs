using Microsoft.EntityFrameworkCore;
using Orpheo.Data;
using Orpheo.Models;
using ORPHEO.Services;
using Microsoft.AspNetCore.Identity;    
using Microsoft.AspNetCore.Identity.UI;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddScoped<ISongAiTagService, GoogleSongAiTagService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        using var scope = context.RequestServices.CreateScope();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var user = await userManager.GetUserAsync(context.User);

        if (user != null && user.UserCode == null)
        {
            string code;
            do
            {
                code = Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 8)
                    .ToUpper();
            }
            while (await userManager.Users.AnyAsync(u => u.UserCode == code));

            user.UserCode = code;
            await userManager.UpdateAsync(user);
        }
    }

    await next();
});


app.UseAuthorization();

app.MapHub<Orpheo.Hubs.SessionRoomHub>("/sessionRoomHub");

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
