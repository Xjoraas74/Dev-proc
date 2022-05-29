using Dev_proc.Constants.Configuration;
using Dev_proc.Data;
using Dev_proc.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
var roles = context.Roles.ToList();
var users = context.Users.Include(x => x.Roles).ToList();
var systemUsers = builder.Configuration.GetSection("SystemUsers");

foreach (var roleType in Enum.GetValues<RoleType>())
{
    var role = roles.Where(x => x.Type == roleType)
        .FirstOrDefault();
    if(role == null)
    {
        context.Roles.Add(new Role
        {
            Name = roleType.ToString(),
            NormalizedName = roleType.ToString().ToUpper(),
            Type = roleType
        });
    }
    var systemUser = systemUsers.GetSection(roleType.ToString());
    var user = users.Where(u => u.Roles.Any(r => r.Role.Type == roleType)).FirstOrDefault();
    if(user == null)
    {
        Console.WriteLine("xd");
    }
}
context.SaveChanges();


app.Run();
