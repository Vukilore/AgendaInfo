using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class Agenda
    {
        /***************************PROPRIETES*******************************/
        public List<DayOff> ListDaysOff { get; set; }                   // Liste des jours de congés du technicien
        public List<Service> ListTarifService { get; set; }             // Liste des services proposés par le technicien
        public List<RendezVous> ListScheduledRendezVous { get; set; }   // Liste des rendez-vous programmés
        public List<Evaluation> ListEvaluations { get; set; }           // Liste des évaluations 
                                                                         
        /***************************Constructeur*******************************/

        /***************************METHODES*******************************/

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
         * AddService: Ajoute un rdv à la Liste
         *=========================================*/
        public void AddRendezVous(RendezVous rendezvous)
        {
            ListScheduledRendezVous.Add(rendezvous);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteService: Supprime un rdv à la Liste
         *=========================================*/
        public void DeleteRendezVous(RendezVous rendezvous)
        {
            ListScheduledRendezVous.Remove(rendezvous);
            // TODO: appel de la DAL
        }

        /*=========================================
         * AddService: Ajoute une évaluation à la Liste
         *=========================================*/
        public void AddEvaluation(Evaluation evaluation)
        {
            ListEvaluations.Add(evaluation);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteService: Supprime une évaluation  à la Liste
         *=========================================*/
        public void DeleteREvaluation(Evaluation evaluation)
        {
            ListEvaluations.Remove(evaluation);
            // TODO: appel de la DAL
        }
    }
}
