using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EmailSettings from appsettings.json
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register EmailService
builder.Services.AddScoped<EmailService>(serviceProvider =>
{
    var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
    return new EmailService(emailSettings!);
});

var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        
        // Seed data if database is empty
        SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Seed data method
static void SeedData(ApplicationDbContext context)
{
    // Seed Regions if empty
    if (!context.Regions.Any())
    {
        context.Regions.AddRange(
            new Region { RegionId = 1, Name = "Đồng bằng sông hồng" },
            new Region { RegionId = 2, Name = "Đồng bằng sông cửu long" },
            new Region { RegionId = 3, Name = "Bắc Trung Bộ" },
            new Region { RegionId = 4, Name = "Nam Trung Bộ" },
            new Region { RegionId = 5, Name = "Tây Nguyên" }
        );
    }

    // Seed Roles if empty
    if (!context.Roles.Any())
    {
        context.Roles.AddRange(
            new Role { roleId = 1, Name = "Admin" },
            new Role { roleId = 2, Name = "User" },
            new Role { roleId = 3, Name = "Moderator" }
        );
    }

    // Seed GameLevels if empty
    if (!context.GameLevels.Any())
    {
        context.GameLevels.AddRange(
            new GameLevel { LevelId = 1, title = "Cấp độ 1", Description = "Cấp độ dễ cho người mới bắt đầu" },
            new GameLevel { LevelId = 2, title = "Cấp độ 2", Description = "Cấp độ trung bình" },
            new GameLevel { LevelId = 3, title = "Cấp độ 3", Description = "Cấp độ khó" }
        );
    }

    // Seed Questions if empty
    if (!context.Questions.Any())
    {
        context.Questions.AddRange(
            new Question
            {
                QuestionId = 1,
                ContentQuestion = "Thủ đô của Việt Nam là gì?",
                Answer = "Hà Nội",
                Option1 = "Hà Nội",
                Option2 = "Hồ Chí Minh",
                Option3 = "Đà Nẵng",
                Option4 = "Huế",
                levelId = 1
            },
            new Question
            {
                QuestionId = 2,
                ContentQuestion = "Sông nào dài nhất Việt Nam?",
                Answer = "Sông Đồng Nai",
                Option1 = "Sông Hồng",
                Option2 = "Sông Đồng Nai",
                Option3 = "Sông Cửu Long",
                Option4 = "Sông Mê Kông",
                levelId = 1
            },
            new Question
            {
                QuestionId = 3,
                ContentQuestion = "Núi nào cao nhất Việt Nam?",
                Answer = "Fansipan",
                Option1 = "Bà Đen",
                Option2 = "Fansipan",
                Option3 = "Bạch Mã",
                Option4 = "Lang Biang",
                levelId = 2
            },
            new Question
            {
                QuestionId = 4,
                ContentQuestion = "Việt Nam có bao nhiêu tỉnh thành?",
                Answer = "63",
                Option1 = "58",
                Option2 = "63",
                Option3 = "64",
                Option4 = "65",
                levelId = 2
            },
            new Question
            {
                QuestionId = 5,
                ContentQuestion = "Di sản văn hóa nào của Việt Nam được UNESCO công nhận đầu tiên?",
                Answer = "Quần thể di tích Cố đô Huế",
                Option1 = "Vịnh Hạ Long",
                Option2 = "Phố cổ Hội An",
                Option3 = "Quần thể di tích Cố đô Huế",
                Option4 = "Thánh địa Mỹ Sơn",
                levelId = 3
            },
            new Question
            {
                QuestionId = 6,
                ContentQuestion = "Năm nào Việt Nam chính thức thống nhất?",
                Answer = "1975",
                Option1 = "1973",
                Option2 = "1974",
                Option3 = "1975",
                Option4 = "1976",
                levelId = 3
            }
        );
    }

    // Save changes to ensure Regions and Roles are in database before adding Users
    context.SaveChanges();

    // Seed Users if empty
    if (!context.LegacyUsers.Any())
    {
        context.LegacyUsers.AddRange(
            new User 
            { 
                userId = 1, 
                username = "admin", 
                linkAvatar = "https://i.pravatar.cc/150?img=1", 
                otp = 0,
                region = context.Regions.First(r => r.RegionId == 1),
                role = context.Roles.First(r => r.roleId == 1)
            },
            new User 
            { 
                userId = 2, 
                username = "nguyenvana", 
                linkAvatar = "https://i.pravatar.cc/150?img=2", 
                otp = 1234,
                region = context.Regions.First(r => r.RegionId == 1),
                role = context.Roles.First(r => r.roleId == 2)
            },
            new User 
            { 
                userId = 3, 
                username = "tranvanb", 
                linkAvatar = "https://i.pravatar.cc/150?img=3", 
                otp = 5678,
                region = context.Regions.First(r => r.RegionId == 2),
                role = context.Roles.First(r => r.roleId == 2)
            },
            new User 
            { 
                userId = 4, 
                username = "levanc", 
                linkAvatar = "https://i.pravatar.cc/150?img=4", 
                otp = 9012,
                region = context.Regions.First(r => r.RegionId == 3),
                role = context.Roles.First(r => r.roleId == 2)
            },
            new User 
            { 
                userId = 5, 
                username = "phamvand", 
                linkAvatar = "https://i.pravatar.cc/150?img=5", 
                otp = 3456,
                region = context.Regions.First(r => r.RegionId == 4),
                role = context.Roles.First(r => r.roleId == 3)
            },
            new User 
            { 
                userId = 6, 
                username = "hoangvane", 
                linkAvatar = "https://i.pravatar.cc/150?img=6", 
                otp = 7890,
                region = context.Regions.First(r => r.RegionId == 5),
                role = context.Roles.First(r => r.roleId == 2)
            }
        );
    }

    context.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
