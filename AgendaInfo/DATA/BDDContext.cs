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
        public DbSet<User>      ContextUser;
        public DbSet<Customer>  ContextCustomer;
        public DbSet<Admin>     ContextAdmin;
        public DbSet<Evaluation> ContextEvaluation;
        public DbSet<Service> ContextService;
    }
}
