namespace TaskManager.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=TaskManagerDB")
        {
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TaskPriority> TaskPriorities { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskStatus> TaskStatuses { get; set; }
        public virtual DbSet<TaskType> TaskTypes { get; set; }
        public virtual DbSet<UserData> UserDatas { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskPriority>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.TaskPriority)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskStatus>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.TaskStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskType>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.TaskType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserData>()
                .Property(e => e.Phone)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserData>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserData)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
