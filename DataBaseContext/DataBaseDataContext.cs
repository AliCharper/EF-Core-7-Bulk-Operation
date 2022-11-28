using EFBulkActivities_V2.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EFBulkActivities_V2.DataBaseContext
{
    public class DataBaseDataContext : DbContext
    {
        public DataBaseDataContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UniversityClass>(builder =>
            {
                builder.ToTable("UniversityClass");

                builder
                    .HasMany(uniClass => uniClass.Students)
                    .WithOne()
                    .HasForeignKey(student => student.ClassID)
                    .IsRequired();

                builder.HasData(new UniversityClass
                {
                    ID = 1,
                    Name = "C# Class",
                });
            });

            modelBuilder.Entity<Student>(builder =>
            {
                builder.ToTable("Student");

                var Students = Enumerable
                    .Range(1, 1000)
                    .Select(id => new Student
                    {
                        ID = id,
                        Name = $"Student #{id}",
                        CPGA = 3.5m,
                        ClassID = 1
                    })
                    .ToList();

                builder.HasData(Students);
            });
        }


    }
}
