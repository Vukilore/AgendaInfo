using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class Customer : User
    {
        /***************************PROPRIETES*******************************/
        public virtual List<RendezVous> ListRendezVous { get; set; }    // Liste des rendez-vous du client
        public virtual List<Evaluation> ListEvaluation { get; set; }    // Liste des évaluations du client

        /***************************Constructeur*******************************/
        public Customer(int i, string n, string l, DateTime b, string a, int ph, string e, string pa)
            : base(i, n, l, b, a, ph, e, pa) { }

        /***************************METHODES*******************************/

        /*=========================================
         * AddService: Ajoute un rdv à la Liste
         *=========================================*/
        public void AddRendezVous(RendezVous rendezvous)
        {
            ListRendezVous.Add(rendezvous);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteService: Supprime un rdv à la Liste
         *=========================================*/
        public void DeleteRendezVous(RendezVous rendezvous)
        {
            ListRendezVous.Remove(rendezvous);
            // TODO: appel de la DAL
        }

        /*=========================================
         * AddService: Ajoute une évaluation à la Liste
         *=========================================*/
        public void AddEvaluation(Evaluation evaluation)
        {
            ListEvaluation.Add(evaluation);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteService: Supprime une évaluation  à la Liste
         *=========================================*/
        public void DeleteREvaluation(Evaluation evaluation)
        {
            ListEvaluation.Remove(evaluation);
            // TODO: appel de la DAL
        }
    }
}
