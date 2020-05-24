using AgendaInfo.DATA;
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
        public Customer() { }
        public Customer(string _email) : base(_email) { }


        /***************************METHODES*******************************/

        /*=========================================
         * Update: Met à jour le client dans la bdd
         * *=========================================*/
        public void Update(IUserDAL userDAL)
        {
            userDAL.Update(this);
        }

        /*=========================================
         * Register: Enregister le client dans la BDD
         *=========================================*/
        public void Register(IUserDAL userDAL)
        {                                                                      
            userDAL.Add(this);
        }

        /*=========================================
         * Exist: Vérifie si l'email de l'user existe dans la BDD
         *=========================================*/
        public bool Exist(IUserDAL userDAL)
        {
            return userDAL.Exist(this);
        }



        /*=========================================
         * EditEvaluation: modifie une évaluation dans la liste
         *=========================================*/
        public void EditEvaluation(Evaluation evaluation, IUserDAL userDAL)
        {
            int index = ListEvaluation.FindIndex(m => m.ID == evaluation.ID);
            if (index >= 0)
            {
                ListEvaluation[index].Rate = evaluation.Rate;
                ListEvaluation[index].Comment = evaluation.Comment;
            }
            else throw new Exception("Impossible d'editer cette evaluation, elle n'existe pas");
            userDAL.Update(this);
        }

        /*=========================================
         * AddEvaluation: Ajoute une évaluation à la Liste
         *=========================================*/
        public void AddEvaluation(Evaluation evaluation, IUserDAL userDAL)
        {
            ListEvaluation.Add(evaluation);
            userDAL.Update(this);
        }

        /*=========================================
         * DeleteEvaluation: Supprime une évaluation  à la Liste
         *=========================================*/
        public void DeleteEvaluation(Evaluation evaluation, IUserDAL userDAL, IEvalDAL evalDAL)
        {
            ListEvaluation.Remove(evaluation);
            userDAL.Update(this);
            evalDAL.Delete(evaluation);
        }



        /*=========================================
         * AddRendezVous: Ajouter un rendez-vous  à la Liste 
         *=========================================*/
        public void AddRendezVous(RendezVous rendezvous, IUserDAL userDAL)
        {
            ListRendezVous.Add(rendezvous);
            userDAL.Update(this);
        }

        /*=========================================
         * DeleteRendezVous: Supprime un rendez-vous  à la Liste
         *=========================================*/
        public void DeleteRendezVous(RendezVous rendezvous, IUserDAL userDAL, IRendezVousDAL rdvDAL)
        {
            ListRendezVous.Remove(rendezvous);
            userDAL.Update(this);
            rdvDAL.Delete(rendezvous);
        }

        
    }
}
