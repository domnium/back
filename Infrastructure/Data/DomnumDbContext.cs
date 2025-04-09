using System;
using System.Reflection;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.Entities.Payments;
using Domain.Entities.Relationships;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DomnumDbContext : DbContext 
{
    public DomnumDbContext(DbContextOptions<DomnumDbContext> options)
        : base(options)
    {
    }
    public DbSet<StudentLecture> StudentLectures { get; init; }
    public DbSet<StudentCourse> StudentCourses { get; init; }
    public DbSet<Student> Students { get; init; }
    public DbSet<Course> Courses { get; init; }
    public DbSet<Domain.Entities.Core.Module> Modules { get; init; }
    public DbSet<Teacher> Teachers { get; init; }
    public DbSet<Lecture> Lectures { get; init; }
    public DbSet<Parameter> Parameters { get; init; }
    public DbSet<Video> Videos { get; init; }
    public DbSet<Category> Categories { get; init; }
    public DbSet<IA> IAs { get; init; }
    public DbSet<Picture> Pictures { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<FreeSubscription> FreeSubscriptions { get; init; }
    public DbSet<PremiumSubscription> PremiumSubscriptions { get; init; }
    public DbSet<StripeWebhookEvent> StripeWebhookEvents { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Notification>();
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}