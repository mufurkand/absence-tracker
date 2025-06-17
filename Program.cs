using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using absence_tracker.Data;
using absence_tracker.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Configure Entity Framework
builder.Services.AddDbContext<AbsenceTrackerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AbsenceTrackerDbContext>()
.AddDefaultTokenProviders();

// Register services
builder.Services.AddScoped<absence_tracker.Services.IUserService, absence_tracker.Services.UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed data
// We are seeding data here and not with onConfiguring because we use Identity and need UserManager
// to create the admin user and hash the password correctly.
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await SeedDataAsync(services);

app.Run();

async Task SeedDataAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    var context = serviceProvider.GetRequiredService<AbsenceTrackerDbContext>();

    // Check if admin user already exists
    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        // Create admin user with proper password hashing and normalization
        adminUser = new User
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin123!");

        if (result.Succeeded)
        {
            // Seed courses for the admin user
            var courses = new[]
            {
                new Course
                {
                    Name = "Mathematics",
                    Description = "Advanced Mathematics Course",
                    UserId = adminUser.Id,
                    TotalAbsences = 0
                },
                new Course
                {
                    Name = "Physics",
                    Description = "Introduction to Physics",
                    UserId = adminUser.Id,
                    TotalAbsences = 0
                }
            };

            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();
        }
    }
}
