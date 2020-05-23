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
                        Name = "Partou",
                        FirstName = "Rémy",
                        Email = "remypartou@condorcet.be",
                        Password = "123456",
                        Address = "Rue de l'ecole",
                        Birthday = new DateTime(1994, 21, 04),
                        PhoneNumber = 6546546
                    });

                    // Création des utilisateurs
                    c1 = new Customer
                    {
                        Name = "Masset",
                        FirstName = "Laurent",
                        Email = "lmasser@condorcet.be",
                        Password = "1234",
                        Address = "Rue de la montagne",
                        Birthday = new DateTime(1902, 03, 07),
                    };
                    context.User.Add(c1);
                    c2 = new Customer
                    {
                        Name = "Copin",
                        FirstName = "Brigitte",
                        Email = "bcopin@condorcet.be",
                        Password = "5678",
                        Address = "Chemin du milieu",
                        Birthday = new DateTime(1902, 03, 07),
                    };
                    context.User.Add(c2);
                    c3 = new Customer
                    {
                        Name = "Vandevorst",
                        FirstName = "Anne",
                        Email = "avdv@condorcet.be",
                        Password = "5678",
                        Address = "Rue de l'automne",
                        Birthday = new DateTime(1902, 03, 07),
                    };
                    context.User.Add(c3);
                    c4 = new Customer
                    {
                        Name = "Clini",
                        FirstName = "Dorian",
                        Email = "dclini@condorcet.be",
                        Password = "1234",
                        Address = "Rue du berger",
                        Birthday = new DateTime(1902, 03, 07),
                    };
                    context.User.Add(c4);
                }

                // Création des services
                if (!context.Service.Any())
                {
                    s1 = new Service
                    {
                        Name = "Formatage",
                        Price = 30,
                        Duration = 2,
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

                // Création de rendez-vous
                if (!context.RendezVous.Any())
                {
                    context.RendezVous.Add(new RendezVous
                    {
                        Customer = c1,
                        Service = s2,
                        Comment = "Gros chien à l'entrée",
                        BeginDate = new DateTime(2020, 28, 05, 10, 0, 0),
                    });
                    context.RendezVous.Add(new RendezVous
                    {
                        Customer = c4,
                        Service = s3,
                        Comment = "Aucune",
                        BeginDate = new DateTime(2020, 29, 05, 16, 0, 0),
                    });
                    context.RendezVous.Add(new RendezVous
                    {
                        Customer = c2,
                        Service = s1,
                        Comment = "La maison se trouve dans le fond de l'allée",
                        BeginDate = new DateTime(2020, 05, 06, 13, 0, 0),
                    });
                    context.RendezVous.Add(new RendezVous
                    {
                        Customer = c3,
                        Service = s2,
                        Comment = "Chien dangereux",
                        BeginDate = new DateTime(2020, 11, 06, 9, 0, 0),
                    });
                }

                // Création des évaluations
                if (!context.Evaluation.Any())
                {
                    context.Evaluation.Add(new Evaluation
                    {
                        Rate = 4,
                        Comment = "Super, rapide et efficace",
                    });
                    context.Evaluation.Add(new Evaluation
                    {
                        Rate = 3,
                        Comment = "Est arrivé en retard mais a été efficace",
                    });
                    context.Evaluation.Add(new Evaluation
                    {
                        Rate = 5,
                        Comment = "Rien à redire, très professionel",
                    });
                }
                // Création des jours de congé 
                if (!context.DayOff.Any())
                {
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 11, 06, 12, 0, 0),
                        Reason = "Personnel",
                    });
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 11, 06, 13, 0, 0),
                        Reason = "Personnel",
                    });
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 28, 05, 8, 0, 0),
                        Reason = "Aucune",
                    });
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 28, 05, 9, 0, 0),
                        Reason = "Aucune",
                    });
                }


                context.SaveChanges();

            }
        }
    }
}
