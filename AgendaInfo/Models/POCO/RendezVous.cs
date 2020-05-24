using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class RendezVous
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }                         // ID du rendez-vous

        [Display(Name = "Auteur du rendez-vous")]
        [BindProperty(Name = "Customer")]
        public virtual Customer Customer { get; set; }      // L'auteur du rendez-vous
        public virtual Service Service { get; set; }        // Le service choisis

        [Display(Name = "Commentaire supplémentaire concernant le rendez-vous (non obligatoire)")]
        [MaxLength(244, ErrorMessage = "244 caractère maximum")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }                 // Un commentaire supplémentaire de la part du client

        [DataType(DataType.DateTime)]
        [Display(Name = "Date / Heure du rendez-vous")]
        public DateTime BeginDate { get; set; }             // Date/heure de début du rendez-vous
        
        /***************************Constructeur*******************************/
        public RendezVous() { }
        public RendezVous (Customer _customer, Service _service, string _comment, DateTime _beginDate)
	    {
            Customer = _customer;
            Service =  _service;
            Comment=   _comment;
            BeginDate= _beginDate;
	    }
                                                                  
        // Nous aide pour récuperer son ID depuis un formulaire
        public override string ToString()
        {
            return $"{ID}";
        }
    }
    
}
