using Agenda.Models.POCO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BDDContext(serviceProvider.GetRequiredService<DbContextOptions<BDDContext>>()))
            {
                if (!context.User.Any())
                {
                    // Création de l'administrateur
                    context.User.Add(new Admin
                    {
                        Name = "Doe",
                        FirstName = "John",
                        Email = "johndoe@condorcet.be",
                        Password = "123456",
                        Address = "Rue de l'ecole",
                        Birthday = DateTime.Now,
                        PhoneNumber = 6546546
                    }
                    context.User.Add(new Customer
                    {

                    }
                    );
                    if (!context)
                if(!context.RendezVous.Any())
                {
                    context.RendezVous.Add(new RendezVous
                    {
                        // atributs
                    }
                }
                
                    context.SaveChanges();
                }
            }
        }
    }
}
