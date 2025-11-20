using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Region> Regions { get; set; }
    public DbSet<GameLevel> GameLevels { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<LevelResult> LevelResults { get; set; }
    public DbSet<User> LegacyUsers { get; set; }
    public new DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId);
            entity.Property(e => e.Name).IsRequired();
        });
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.roleId);
            entity.Property(e => e.Name).IsRequired();
        });
        // Optional: Configure your entity here
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("LegacyUsers");
            entity.HasKey(e => e.userId);
            entity.Property(e => e.username).IsRequired();
            entity.Property(e => e.linkAvatar);
            entity.Property(e => e.otp);

            entity.HasOne(e => e.region)
                  .WithMany(r => r.Users)
                  .HasForeignKey("regionId")
                  .IsRequired();

            entity.HasOne(e => e.role)
                  .WithMany(r => r.Users)
                  .HasForeignKey("roleId")
                  .IsRequired();
        });
        


        // Seed data
        modelBuilder.Entity<GameLevel>().HasData(
            new GameLevel { LevelId = 1, title = "Cấp độ 1", Description = "Cấp độ dễ cho người mới bắt đầu" },
            new GameLevel { LevelId = 2, title = "Cấp độ 2", Description = "Cấp độ trung bình" },
            new GameLevel { LevelId = 3, title = "Cấp độ 3", Description = "Cấp độ khó" }
        );

        modelBuilder.Entity<Region>().HasData(
            new Region { RegionId = 1, Name = "Đồng bằng sông hồng" },
            new Region { RegionId = 2, Name = "Đồng bằng sông cửu long" },
            new Region { RegionId = 3, Name = "Bắc Trung Bộ" },
            new Region { RegionId = 4, Name = "Nam Trung Bộ" },
            new Region { RegionId = 5, Name = "Tây Nguyên" }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { roleId = 1, Name = "Admin" },
            new Role { roleId = 2, Name = "User" },
            new Role { roleId = 3, Name = "Moderator" }
        );

        modelBuilder.Entity<Question>().HasData(
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
}
