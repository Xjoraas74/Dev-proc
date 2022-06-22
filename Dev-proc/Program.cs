using Dev_proc.Data;
using Dev_proc.Models.CompanyModels;
using Dev_proc.Models.DeanModels;
using Dev_proc.Models.Identity;
using Dev_proc.Services;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddScoped<IFileService, FileService>();
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



//TODO: Move this BL in service
using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
context.Database.Migrate(); // create and migrate a database
var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
var roles = context.Roles.ToList();
var users = context.Users.Include(x => x.Roles).ToList();
var systemUsers = builder.Configuration.GetSection("SystemUsers");

foreach (var roleType in Enum.GetValues<RoleType>())
{
    var role = roles.Where(x => x.Type == roleType)
        .FirstOrDefault();
    if(role == null)
    {
        role = new Role
        {
            Name = roleType.ToString(),
            NormalizedName = roleType.ToString().ToUpper(),
            Type = roleType
        };
        context.Roles.Add(role);
    }
    var systemUser = systemUsers.GetSection(roleType.ToString());
    var user = users.Where(u => u.Roles.Any(r => r.Role.Type == roleType)).FirstOrDefault();
    if(user == null)
    {
        var systemUserLogin = systemUser.GetSection("Login").Value;
        var systemUserPassword = systemUser.GetSection("Password").Value;
        if(systemUserLogin == null || systemUserPassword == null)
        {
            continue;
        }

        user = new User
        {
            Email = systemUserLogin,
            UserName = systemUserLogin,
            EmailConfirmed = true,
        };
        var userRole = new UserRole
        {
            Role = role,
            User = user
        };
        if (roleType == RoleType.Company)
        {
            var company = new Company
            {
                User = user,
                Name = "ÓÖÐ)"
            };
            user.Company = company;
            context.Companies.Add(company);
        }
        if (roleType == RoleType.Company)
        {
            var company = new Company
            {
                User = user,
                Name = "ÓÖÐ)"
            };
            user.Company = company;
            context.Companies.Add(company);
        }
        if(roleType == RoleType.Dean)
        {
            var dean = new Dean
            {
                User = user,
                Name = "ÒÃÓ"
            };
            user.Dean = dean;
            context.Deans.Add(dean);
        }
        user.Roles = new List<UserRole>() { userRole };
        await userManager.CreateAsync(user, systemUserPassword);
    }
}
context.SaveChanges();


app.Run();
