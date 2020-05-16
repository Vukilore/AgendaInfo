using AgendaInfo.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class Agenda
    {

        /***************************PROPRIETES*******************************/
        public virtual List<DayOff> ListDaysOff { get; set; }                   // Liste des jours de congés du technicien
        public virtual List<Service> ListTarifService { get; set; }             // Liste des services proposés par le technicien
        public virtual List<RendezVous> ListRendezVous { get; set; }            // Liste des rendez-vous programmés
        public virtual List<Evaluation> ListEvaluations { get; set; }           // Liste des évaluations 
        private static Agenda instance = null;

        /***************************Constructeur*******************************/
        private Agenda()
        {
            
        }


        public static Agenda GetInstance()
        {
            if (instance == null)
                instance = new Agenda();
            return instance;
        }

        /***************************METHODES*******************************/
        public static List<RendezVous> GetListOfRDV(IRendezVousDAL rdvDAL) => rdvDAL.GetAll();

        /*=========================================
         * AddDayOff: Ajoute un congé à la Liste
         *=========================================*/
        public void AddDayOff(DayOff dayoff)
        {
            ListDaysOff.Add(dayoff);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteDayOff: Supprime un congé à la Liste
         *=========================================*/
        public void DeleteDayOff(DayOff dayoff)
        {
            ListDaysOff.Remove(dayoff);
            // TODO: appel de la DAL
        }

        /*=========================================
         * AddService: Ajoute un service à la Liste
         *=========================================*/
        public void AddService(Service service)
        {
            ListTarifService.Add(service);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteService: Supprime un congé à la Liste
         *=========================================*/
        public void DeleteService(Service service)
        {
            ListTarifService.Remove(service);
            // TODO: appel de la DAL
        }

        /*=========================================
         * AddRendezVous: Ajoute un rdv à la Liste
         *=========================================*/
        public void AddRendezVous(RendezVous rendezvous)
        {
            ListRendezVous.Add(rendezvous);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteRendezVous: Supprime un rdv à la Liste
         *=========================================*/
        public void DeleteRendezVous(RendezVous rendezvous)
        {
            ListRendezVous.Remove(rendezvous);
            // TODO: appel de la DAL
        }

        /*=========================================
         * AddEvaluation: Ajoute une évaluation à la Liste
         *=========================================*/
        public void AddEvaluation(Evaluation evaluation)
        {
            ListEvaluations.Add(evaluation);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteEvaluation: Supprime une évaluation  à la Liste
         *=========================================*/
        public void DeleteEvaluation(Evaluation evaluation)
        {
            ListEvaluations.Remove(evaluation);
            // TODO: appel de la DAL
        }
    }
}
