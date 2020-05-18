using AgendaInfo.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Agenda.Models.POCO
{
    public class Agenda
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }
        public virtual List<DayOff> ListDaysOff { get; set; }                   // Liste des jours de congés du technicien
        public virtual List<Service> ListServices { get; set; }                 // Liste des services proposés par le technicien
        public virtual List<RendezVous> ListRendezVous { get; set; }            // Liste des rendez-vous programmés
        public virtual List<Evaluation> ListEvaluations { get; set; }           // Liste des évaluations 

        private static Agenda instance = null;

        /***************************Constructeur*******************************/
        public Agenda()
        {

        }

        public static Agenda GetInstance()
        {
            if (instance == null)
                instance =  new Agenda();
            return instance;
        }

        /***************************METHODES*******************************/


        /*=========================================
         * UpdateRDV: Met à jour la liste des RDV
         *=========================================*/
        public void UpdateRDV(IRendezVousDAL rdvDAL) => ListRendezVous = rdvDAL.GetAll();

        /*=========================================
         * AddDayOff: Ajoute un congé à la Liste
         *=========================================*/
        public void AddDayOff(DayOff dayoff)
        {
            ListDaysOff.Add(dayoff);
        }

        /*=========================================
         * DeleteDayOff: Supprime un congé à la Liste
         *=========================================*/
        public void DeleteDayOff(DayOff dayoff)
        {
            ListDaysOff.Remove(dayoff);

        }

        /*=========================================
         * AddService: Ajoute un service à la Liste
         *=========================================*/
        public void AddService(Service service)
        {
            ListServices.Add(service);
        }

        /*=========================================
         * DeleteService: Supprime un congé à la Liste
         *=========================================*/
        public void DeleteService(Service service)
        {
            ListServices.Remove(service);
        }

        /*=========================================
         * AddRendezVous: Ajoute un rdv à la Liste
         *=========================================*/
        public void AddRendezVous(RendezVous rendezvous)
        {
            ListRendezVous.Add(rendezvous);

        }

        /*=========================================
         * DeleteRendezVous: Supprime un rdv à la Liste
         *=========================================*/
        public void DeleteRendezVous(RendezVous rendezvous)
        {
            ListRendezVous.Remove(rendezvous);

        }

        /*=========================================
         * AddEvaluation: Ajoute une évaluation à la Liste
         *=========================================*/
        public void AddEvaluation(Evaluation evaluation)
        {
            ListEvaluations.Add(evaluation);

        }

        /*=========================================
         * DeleteEvaluation: Supprime une évaluation  à la Liste
         *=========================================*/
        public void DeleteEvaluation(Evaluation evaluation)
        {
            ListEvaluations.Remove(evaluation);

        }
    }
}
