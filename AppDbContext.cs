using Fitness.Goal.GoalType;
using Microsoft.EntityFrameworkCore;

namespace Fitness;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User.User> Users { get; set; }

    public DbSet<GoalType> GoalTypes { get; set; }

    public DbSet<Goal.Goal> Goals { get; set; }

    public DbSet<Product.Product> Products { get; set; }

    public DbSet<Diet.Diet> Diets { get; set; }

    public DbSet<WaterDiet.WaterDiet> WaterDiet { get; set; }

    public DbSet<Recipe.Recipe> Recipes { get; set; }

    public DbSet<Notification.Notification> Notifications { get; set; }

    public DbSet<Training.Training> Trainings { get; set; }
    
    public DbSet<UserWeight.UserWeight> UserWeights { get; set; }
    
    public DbSet<Sleep.Sleep> Sleeps { get; set; }
    
    public DbSet<Achievement.Achievement> Achievements { get; set; }

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

            // Goal

            modelBuilder.Entity<User.User>()
                .HasOne(u => u.Goal)
                .WithOne()
                .HasForeignKey<User.User>(u => u.GoalId);

            // Weights
            
            modelBuilder.Entity<User.User>()
                .HasMany(u => u.Weights)
                .WithOne(uw => uw.User)
                .HasForeignKey(uw => uw.UserId);
            
            // Diets

            modelBuilder.Entity<User.User>()
                .HasMany(u => u.Diets)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId);
            
            // Water Diets

            modelBuilder.Entity<User.User>()
                .HasMany(u => u.WaterDiets)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId);
            
            // Water Diets

            modelBuilder.Entity<User.User>()
                .HasMany(u => u.Sleeps)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            // Registration and Modification Date

            user.Property(u => u.RegistrationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();

            user.Property(u => u.ModificationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAddOrUpdate();
            
            // Achievement

            modelBuilder.Entity<User.User>()
                .HasMany(u => u.Achievements)
                .WithMany(a => a.Users);


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

        modelBuilder.Entity<Product.Product>(product =>
        {
            // Id

            product.HasIndex(p => p.Id)
                .IsUnique();

            product.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation Date

            product.Property(u => u.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Diet.Diet>(diet =>
        {
            // Id

            diet.HasIndex(p => p.Id)
                .IsUnique();

            diet.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation Date

            diet.Property(u => u.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<WaterDiet.WaterDiet>(wd =>
        {
            // Id

            wd.HasIndex(p => p.Id)
                .IsUnique();

            wd.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation Date

            wd.Property(u => u.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Recipe.Recipe>(r =>
        {
            // Id

            r.HasIndex(p => p.Id)
                .IsUnique();

            r.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation And Modification Date

            r.Property(u => u.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
            
            r.Property(u => u.ModificationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAddOrUpdate();
        });
        
        modelBuilder.Entity<Notification.Notification>(n =>
        {
            // Id

            n.HasIndex(p => p.Id)
                .IsUnique();

            n.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation Date

            n.Property(u => u.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
        });
        
        modelBuilder.Entity<Training.Training>(t =>
        {
            // Id

            t.HasIndex(p => p.Id)
                .IsUnique();

            t.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation And Modification Date

            t.Property(u => u.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
            
            t.Property(u => u.ModificationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAddOrUpdate();
        });
        
        modelBuilder.Entity<Sleep.Sleep>(s =>
        {
            // Id

            s.HasIndex(p => p.Id)
                .IsUnique();

            s.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation And Modification Date

            s.Property(u => u.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
            
            s.Property(u => u.ModificationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAddOrUpdate();
        });
        
        modelBuilder.Entity<Achievement.Achievement>(achievement =>
        {
            // Id

            achievement.HasIndex(a => a.Id)
                .IsUnique();

            achievement.Property(a => a.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Creation And Modification Date

            achievement.Property(a => a.CreationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
            
            achievement.Property(a => a.ModificationDate)
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAddOrUpdate();
        });
    }
}