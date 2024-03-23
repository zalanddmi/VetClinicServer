using Microsoft.EntityFrameworkCore;
using VetClinicServer.Models;

namespace VetClinicServer.Data
{
    public class Context(DbContextOptions<Context> options) : DbContext(options)
    {
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentDrug> AppointmentDrugs { get; set; }
        public DbSet<Species> Species { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppointmentDrug>()
            .HasKey(ad => new { ad.AppointmentId, ad.DrugId });

            modelBuilder.Entity<AppointmentDrug>()
                .HasOne(ad => ad.Appointment)
                .WithMany(a => a.AppointmentDrugs)
                .HasForeignKey(ad => ad.AppointmentId);

            modelBuilder.Entity<AppointmentDrug>()
                .HasOne(ad => ad.Drug)
                .WithMany(d => d.AppointmentDrugs)
                .HasForeignKey(ad => ad.DrugId);
        }
    }
}
