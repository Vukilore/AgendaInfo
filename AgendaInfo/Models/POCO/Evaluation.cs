using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class Evaluation
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }                     // ID de l'évaluation
        public float Rate { get; set; }                 // Note sur 5 de l'evaluation
        public string Comment { get; set; }             // Commentaire sur l'évaluation
        public RendezVous RendezVous { get; set; }      // Rendez-vous concerné par l'évaluation

        /***************************Constructeur*******************************/
        public Evaluation (float _rate, string _comment)
	    {
            Rate = _rate;
            Comment =_comment;
	    }
        /***************************METHODES*******************************/
        public void  FaireEvaluation(){ 
            
            
        }
        
    }
}
