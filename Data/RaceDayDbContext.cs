using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;

namespace RaceDay.Api.Data;

public class RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : DbContext(options)
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<EventCategory> EventCategories => Set<EventCategory>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Result> Results => Set<Result>();
    public DbSet<RouteInformation> RouteInformation => Set<RouteInformation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Role>().HasKey(x => x.RoleID);
        builder.Entity<User>().HasKey(x => x.UserID);
        builder.Entity<Event>().HasKey(x => x.EventID);
        builder.Entity<Category>().HasKey(x => x.CategoryID);
        builder.Entity<EventCategory>().HasKey(x => x.EventCategoryID);
        builder.Entity<Enrolment>().HasKey(x => x.EnrolmentID);
        builder.Entity<Result>().HasKey(x => x.ResultID);
        builder.Entity<RouteInformation>().HasKey(x => x.RouteID);

        builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        builder.Entity<Role>().HasIndex(x => x.RoleName).IsUnique();
        builder.Entity<Category>().HasIndex(x => x.CategoryName).IsUnique();
        builder.Entity<EventCategory>().HasIndex(x => new { x.EventID, x.CategoryID }).IsUnique();
        builder.Entity<Enrolment>().HasIndex(x => new { x.ParticipantID, x.EventCategoryID }).IsUnique();
        builder.Entity<Result>().HasIndex(x => x.EnrolmentID).IsUnique();
        builder.Entity<RouteInformation>().HasIndex(x => x.EventID).IsUnique();

        builder.Entity<Event>()
            .HasOne(x => x.Organiser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<EventCategory>()
            .HasOne(x => x.Event)
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.EventID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<EventCategory>()
            .HasOne(x => x.Category)
            .WithMany(x => x.EventCategories)
            .HasForeignKey(x => x.CategoryID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Enrolment>()
            .HasOne(x => x.Participant)
            .WithMany(x => x.Enrolments)
            .HasForeignKey(x => x.ParticipantID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Enrolment>()
            .HasOne(x => x.EventCategory)
            .WithMany(x => x.Enrolments)
            .HasForeignKey(x => x.EventCategoryID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Result>()
            .HasOne(x => x.Enrolment)
            .WithOne(x => x.Result)
            .HasForeignKey<Result>(x => x.EnrolmentID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RouteInformation>()
            .HasOne(x => x.Event)
            .WithOne(x => x.RouteInformation)
            .HasForeignKey<RouteInformation>(x => x.EventID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Event>().Property(x => x.DistanceKm).HasPrecision(8, 2);
        builder.Entity<EventCategory>().Property(x => x.EntryFee).HasPrecision(10, 2);
        builder.Entity<Result>().Property(x => x.Pace).HasPrecision(8, 2);

        builder.Entity<Role>().HasData(
            new Role { RoleID = 1, RoleName = "Organiser", Description = "Manages events, categories, enrolments and results." },
            new Role { RoleID = 2, RoleName = "Participant", Description = "Browses events, enters events and tracks personal results." });
    }
}
