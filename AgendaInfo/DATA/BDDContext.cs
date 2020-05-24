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
        public DbSet<User>                      User { get; set; }
        public DbSet<Admin>                     Admin { get; set; }
        public DbSet<DayOff>                    DayOff { get; set; }
        public DbSet<Service>                   Service { get; set; }
        public DbSet<Customer>                  Customer { get; set; }
        public DbSet<Evaluation>                Evaluation { get; set; }
        public DbSet<RendezVous>                RendezVous { get; set; }       

        // Configuration du discriminator
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasDiscriminator<string>("Type")
                        .HasValue<Admin>("Admin")
                        .HasValue<Customer>("Customer");
        }
    }
}