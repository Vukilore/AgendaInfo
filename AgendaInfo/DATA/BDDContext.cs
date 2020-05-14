using Agenda.Models.POCO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public class BDDContext : DbContext
    {
        public BDDContext(DbContextOptions<BDDContext> options) : base(options)
        {

        }

        // Création des tables.
        public DbSet<User>       User { get; set; }
        public DbSet<Customer>   Customer { get; set; }
        public DbSet<Admin>      Admin { get; set; }
        public DbSet<Evaluation> Evaluation { get; set; }
        public DbSet<Service>    Service { get; set; }
        public DbSet<Agenda.Models.POCO.Agenda>     Agenda { get; set; }

        // Configuration du discriminator
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasDiscriminator<string>("Type")
                        .HasValue<Admin>("Admin")
                        .HasValue<Customer>("Customer");
            modelBuilder.Entity<Agenda.Models.POCO.Agenda>()
                        .HasNoKey();
        }

        // Configuration du discriminator
        public DbSet<Agenda.Models.POCO.RendezVous> RendezVous { get; set; }
    }
}