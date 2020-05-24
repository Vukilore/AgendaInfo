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
        private static RendezVous rdv1;                                                                                                                       
        private static RendezVous rdv2;
        private static RendezVous rdv3;
        private static RendezVous rdv4;
        

                                                                                                                        
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BDDContext(serviceProvider.GetRequiredService<DbContextOptions<BDDContext>>()))
            {
                List<Service> lst_service = new List<Service>();
                List<Evaluation> lst_eval = new List<Evaluation>();
                List<RendezVous> lst_rendezvous1 = new List<RendezVous>();
                List<RendezVous> lst_rendezvous2 = new List<RendezVous>();         
                List<RendezVous> lst_rendezvous3 = new List<RendezVous>();
                List<RendezVous> lst_rendezvous4 = new List<RendezVous>();

                // Création des services
                Service s1 = new Service
                {
                    Name = "Formatage",
                    Price = 30.0,
                    Duration = 2
                };
                lst_service.Add(s1);
                Service s2 = new Service
                {
                    Name = "Configurer Réseau",
                    Price = 50.0,
                    Duration = 3
                };
                lst_service.Add(s2);
                Service s3 = new Service
                {
                    Name = "Dévirussage",
                    Price = 25.0,
                    Duration = 1
                };
                lst_service.Add(s3);
                // Création de rendez-vous
                rdv1 = new RendezVous
                {
                    Service = s2,
                    Comment = "Gros chien à l'entrée",
                    BeginDate = new DateTime(2020, 05, 28, 10, 0, 0)
                };
                lst_rendezvous1.Add(rdv1);
                rdv2 = new RendezVous
                {
                    Service = s3,
                    Comment = "Aucune",
                    BeginDate = new DateTime(2020, 05, 21, 16, 0, 0)
                };
                lst_rendezvous4.Add(rdv2);
                rdv3 = new RendezVous
                {
                    Service = s1,
                    Comment = "La maison se trouve dans le fond de l'allée",
                    BeginDate = new DateTime(2020, 06, 05, 13, 0, 0)
                };
                lst_rendezvous2.Add(rdv3);
                rdv4 = new RendezVous
                {
                    Service = s2,
                    Comment = "Chien dangereux",
                    BeginDate = new DateTime(2020, 06, 11, 9, 0, 0)
                };
                lst_rendezvous3.Add(rdv4);

                // Création des évaluations
                Evaluation e1 = new Evaluation
                {
                    Rate = 3,
                    Comment = "Est arrivé en retard mais a été efficace",
                    RendezVous = rdv2
                };
                lst_eval.Add(e1);

                if (!context.User.Where(e => e is Admin).Any())
                {
                    // Création de l'administrateur
                    context.User.Add(new Admin
                    {
                        Name = "Partou",
                        FirstName = "Rémy",
                        Email = "remypartou@condorcet.be",
                        Password = "123456",
                        Address = "Rue de l'ecole",
                        Birthday = DateTime.Now,
                        PhoneNumber = 6546546,
                        ListServices = lst_service,

                    });

                    // Création des utilisateurs
                    c1 = new Customer
                    {
                        Name = "Masset",
                        FirstName = "Laurent",
                        Email = "lmasset@condorcet.be",
                        Password = "1234",
                        Address = "Rue de la montagne",
                        Birthday = new DateTime(1902, 03, 07),
                        ListRendezVous = lst_rendezvous1
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
                        ListRendezVous = lst_rendezvous2
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
                        ListRendezVous = lst_rendezvous3
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
                        ListRendezVous = lst_rendezvous4,
                        ListEvaluation = lst_eval
                    };
                    context.User.Add(c4);
                }

                /* Création des évaluations
                if (!context.Evaluation.Any())
                {
                    context.Evaluation.Add(new Evaluation
                    {
                        Rate = 3,
                        Comment = "Est arrivé en retard mais a été efficace",
                        RendezVous = rdv2
                    });                                
                }
                  */
                // Création des jours de congé 
                if (!context.DayOff.Any())
                {
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 06, 11, 12, 0, 0),
                        Reason = "Personnel"
                    });
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 06, 11, 13, 0, 0),
                        Reason = "Personnel"
                    });
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 05, 28, 8, 0, 0),
                        Reason = "Aucune"
                    });
                    context.DayOff.Add(new DayOff
                    {
                        StartDate = new DateTime(2020, 05, 28, 9, 0, 0),
                        Reason = "Aucune"
                    });
                }


                context.SaveChanges();

            }
        }
    }
}
