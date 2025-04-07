using Fitness.Goal.GoalType;
using Microsoft.EntityFrameworkCore;

namespace Fitness;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User.User> Users { get; set; }

    public DbSet<GoalType> GoalTypes { get; set; }

    public DbSet<Goal.Goal> Goals { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<User.User>(user =>
        {
            // Id
            
            user.HasIndex(u => u.Id)
                .IsUnique();
            
            user.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            
            // Username

            user.HasIndex(u => u.Username)
                .IsUnique();

            user.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            user.HasCheckConstraint(
                "CH_User_Username_MinLength",
                "LENGTH(\"Username\") >= 4"
            );
            
            // Password
            
            user.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);
            
            user.HasCheckConstraint(
                "CH_User_Password_MinLength",
                "LENGTH(\"Password\") >= 8"
            );
            
            // FirstName
            
            user.HasCheckConstraint(
                "CH_User_FirstName_MinLength",
                "LENGTH(\"FirstName\") >= 1"
            );

            user.Property(u => u.FirstName)
                .HasMaxLength(100);
            
            // Birthday

            user.Property(u => u.Birthday)
                .IsRequired();
                
            // Height
            
            user.Property(u => u.Height)
                .IsRequired();
            
            // Weight

            user.Property(u => u.Weight)
                .IsRequired();
            
            // Goal
            
            modelBuilder.Entity<User.User>()
                .HasOne(u => u.Goal)
                .WithOne()
                .HasForeignKey<User.User>(u => u.GoalId);
            
            // Registration and Modification Date
            
            user.Property(u => u.RegistrationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            user.Property(u => u.ModificationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAddOrUpdate();
        });
        
        modelBuilder.Entity<GoalType>(goalType =>
        {
            // Id
            
            goalType.HasIndex(gt => gt.Id)
                .IsUnique();
            
            goalType.Property(gt => gt.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            
            // Name

            goalType.HasIndex(gt => gt.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Goal.Goal>(goal =>
        {
            // Id
            
            goal.HasIndex(g => g.Id)
                .IsUnique();
            
            goal.Property(g => g.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        });
    }
}