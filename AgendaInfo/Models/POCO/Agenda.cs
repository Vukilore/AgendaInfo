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
        public virtual List<RendezVous> ListRendezVous { get; set; }            // Liste des rendez-vous programmés
        public virtual List<Evaluation> ListEvaluations { get; set; }           // Liste des évaluations 
        public virtual List<Customer>   ListCustomers { get; set; }             // Liste des clients

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
            Update(rdvDAL);

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
         * GetEvaluation: Retourne une evaluation depuis la liste des evaluations
         *=========================================*/
        public Evaluation GetEvaluation(int id)
        {
            return ListEvaluations.Find(e => e.ID == id);
        }

        /*=========================================
         * GetCustomer: Retourne un client depuis la liste des clients
         *=========================================*/
        public Customer GetCustomer(int id)
        {
            return ListCustomers.Find(c => c.ID == id);
        }

        /*=========================================
         * ThisWeekDayOff: Retourne une liste de congés pour la semaine indiqué
         *=========================================*/
        public List<DayOff> ThisWeekDayOff(DateTime MondayOfWeek, IDayOffDAL dayOffDAL)
        {
            //1. On récupère tous les congés
            Update(dayOffDAL);

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
         * Update: Met à jour la liste des clients
         *=========================================*/
        public void Update(IUserDAL userDAL) => ListCustomers = userDAL.GetAllCustomers();

        /*=========================================
         * Update: Met à jour la liste des évaluations
         *=========================================  */
        public void Update(IEvalDAL evalDAL) => ListEvaluations = evalDAL.GetAll();

        /*=========================================
         * Update: Met à jour la liste des RDV
         *========================================= */
        public void Update(IDayOffDAL dayOffDAL) => ListDaysOff = dayOffDAL.GetAll();

        /*=========================================
         * Update: Met à jour la liste des RDV
         *========================================= */
        public void Update(IRendezVousDAL rdvDAL) => ListRendezVous = rdvDAL.GetAll();    

        /*=========================================
         * AddDayOff: Ajoute un congé à la Liste
         *=========================================  */
        public void AddDayOff(DayOff dayoff, IDayOffDAL dayOffDAL)
        {
            ListDaysOff.Add(dayoff);
            dayOffDAL.Add(dayoff);
        }

        /*=========================================
         * AddCustomer: Ajoute un utilisateur à la liste et l'enregistre
         *=========================================*/
        public void AddCustomer(Customer customer, IUserDAL userDAL)
        {
            ListCustomers.Add(customer);
            customer.Register(userDAL);
        }

        public bool FreeOfRendezVous(DateTime startDate, DateTime endDateTime, IRendezVousDAL rdvDAL)
        {
            // 1. Mise à jour de la liste de rendez-vous
            Update(rdvDAL);
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
            Update(dayOffDAL);
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
