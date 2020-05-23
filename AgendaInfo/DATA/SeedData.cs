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
        private static Customer c1;
        private static Customer c2;
        private static Customer c3;
        private static Customer c4;
        private static Service s1;
        private static Service s2;
        private static Service s3;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BDDContext(serviceProvider.GetRequiredService<DbContextOptions<BDDContext>>()))
            {
                if (!context.User.Any())
                {
                    // Création de l'administrateur
                    context.User.Add(new Admin
                    {
                        Name = "Dubois",
                        FirstName = "Rémy",
                        Email = "remydubois@condorcet.be",
                        Password = "123456",
                        Address = "Rue de l'ecole",
                        Birthday = new DateTime(1994, 21, 04),
                        PhoneNumber = 6546546
                    });
                    // Création des utilisateurs
                    context.User.Add(new Customer
                    {

                    });
                }

                if (!context.Service.Any())
                {
                    s1 = new Service
                    {
                        Name="Formatage",
                        Price=30,
                        Duration=2,
                    };
                    context.Service.Add(s1);
                    s2 = new Service
                    {
                        Name = "Configurer Réseau",
                        Price = 50,
                        Duration = 3,
                    };
                    context.Service.Add(s2);
                    s3 = new Service
                    {
                        Name = "Dévirussage",
                        Price = 25,
                        Duration = 1,
                    };
                    context.Service.Add(s3);
                }

                if(!context.RendezVous.Any())
                {
                        context.RendezVous.Add(new RendezVous
                        {
                            // atributs
                        });

                }

                /* EVALUTATIONS, DAY OFF */
                
                    context.SaveChanges();
                }
            }
        }
    }
}
