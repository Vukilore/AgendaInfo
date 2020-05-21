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
        * ThisWeekRDV: Retourne une liste de RDV pour la semaine indiqué
        *=========================================*/
        public List<RendezVous> ThisWeekRDV(DateTime MondayOfWeek, IRendezVousDAL rdvDAL)
        {
            //1. On récupère tous les rdv
            Agenda.GetInstance().UpdateRDV(rdvDAL);

            // 2. On crée la liste de retour
            List<RendezVous> RdvThisWeek = new List<RendezVous>();

            //3. Pour chaque rdv dans la liste de l'agenda
            foreach (RendezVous rdv in ListRendezVous)
                //5.1. Si le rendez-vous se situe dans la semaine indiqué on l'ajoute à la liste
                if (rdv.BeginDate.Day >= MondayOfWeek.Day && rdv.BeginDate <= (MondayOfWeek.AddDays(7)))
                    RdvThisWeek.Add(rdv);
            return RdvThisWeek;
        }

        /*=========================================
         * ThisWeekDayOff: Retourne une liste de congés pour la semaine indiqué
         *=========================================*/
        public List<DayOff> ThisWeekDayOff(DateTime MondayOfWeek, IDayOffDAL dayOffDAL)
        {
            //1. On récupère tous les congés
            Agenda.GetInstance().UpdateDayOff(dayOffDAL);

            // 2. On crée la liste de retour
            List<DayOff> daysOffThisWeek = new List<DayOff>();

            //3. Pour chaque congés dans la liste de l'agenda
            foreach (DayOff dayOff in ListDaysOff)
                //3.1. Si le congé se situe dans la semaine indiqué on l'ajoute à la liste
                if (dayOff.StartDate.Day >= MondayOfWeek.Day && dayOff.StartDate <= (MondayOfWeek.AddDays(7)))
                    daysOffThisWeek.Add(dayOff);
            return daysOffThisWeek;
        }

        /*=========================================
         * UpdateDayOff: Met à jour la liste des RDV
         *=========================================*/
        public void UpdateDayOff(IDayOffDAL dayOffDAL) => ListDaysOff = dayOffDAL.GetAll();

        /*=========================================
         * UpdateRDV: Met à jour la liste des RDV
         *=========================================*/
        public void UpdateRDV(IRendezVousDAL rdvDAL) => ListRendezVous = rdvDAL.GetAll();

        /*=========================================
         * AddDayOff: Ajoute un congé à la Liste
         *=========================================*/
        public void AddDayOff(DayOff dayoff, IDayOffDAL dayOffDAL)
        {
            ListDaysOff.Add(dayoff);
            dayOffDAL.Add(dayoff);
        }

        public bool FreeOfRendezVous(DateTime startDate, DateTime endDateTime, IRendezVousDAL rdvDAL)
        {
            // 1. Mise à jour de la liste de rendez-vous
            UpdateRDV(rdvDAL);
            //2. Pour chaque rendez vous dans la liste
            foreach (RendezVous rdv in ListRendezVous)
                // 2.1 Si le rendez vous est le même jour qu'un rdv de la liste
                if (startDate.Date == rdv.BeginDate.Date)
                    //2.2. Si le rdv se situe dans un autre rdv de la liste 
                    if (rdv.BeginDate.Hour >= startDate.Hour && rdv.BeginDate.Hour < endDateTime.Hour)
                        return false;
            return true;
        }

        public bool FreeOfDayOff(DateTime beginDate, DateTime endDateTime, IDayOffDAL dayOffDAL)
        {
            // 1. Mise à jour de la liste de congés
            UpdateDayOff(dayOffDAL);
            //2. Pour chaque congés dans la liste
            foreach (DayOff dayOff in ListDaysOff)
                // 2.1 Si le congé est le même jour qu'un rdv de la liste
                if (beginDate.Date == dayOff.StartDate.Date)
                    //2.2. Si le rdv se situe dans un autre congé de la liste 
                    if (dayOff.StartDate.Hour >= beginDate.Hour && dayOff.StartDate.Hour < endDateTime.Hour)
                        return false;
            return true;
        }

        /*=========================================
         * DeleteDayOff: Supprime un congé à la Liste
         *=========================================*/
        public void DeleteDayOff(DayOff dayoff, IDayOffDAL dayOffDAL)
        {
            ListDaysOff.Remove(dayoff);
            dayOffDAL.Delete(dayoff);
        }

        /*=========================================
         * AddRendezVous: Ajoute un rdv à la Liste
         *=========================================*/
        public void AddRendezVous(RendezVous rendezvous, IRendezVousDAL rdvDAl)
        {
            ListRendezVous.Add(rendezvous);
            rdvDAl.Add(rendezvous);
        }

        /*=========================================
         * DeleteRendezVous: Supprime un rdv à la Liste
         *=========================================*/
        public void DeleteRendezVous(RendezVous rendezvous, IRendezVousDAL rdvDAL)
        {
            ListRendezVous.Remove(rendezvous);
            rdvDAL.Delete(rendezvous);
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
