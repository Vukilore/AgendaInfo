using AgendaInfo.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Agenda.Models.POCO
{
    public class Evaluation
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }                     // ID de l'évaluation
        [Required]
        [Range(0.0,5.0,ErrorMessage ="La note doit être comprise entre 0 et 5")]
        [Display(Name ="Note sur 5")]
        public float Rate { get; set; }                 // Note sur 5 de l'evaluation
        [MaxLength(244,ErrorMessage ="Votre commentaire ne doit pas dépasser 244 caractères")]
        [Display(Name ="Commentaire")]
        public string Comment { get; set; }             // Commentaire sur l'évaluation
        public virtual RendezVous RendezVous { get; set; }      // Rendez-vous concerné par l'évaluation

        /***************************Constructeur*******************************/
        public Evaluation() { }
        public Evaluation (float _rate, string _comment)
	    {
            Rate = _rate;
            Comment =_comment;
	    }

        public void Add(IEvalDAL evalDAL)
        {
            evalDAL.Add(this);
        }
        /***************************METHODES*******************************/


    }
}
