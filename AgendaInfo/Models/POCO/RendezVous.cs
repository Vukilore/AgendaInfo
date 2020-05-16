using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class RendezVous
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }                         // ID du rendez-vous
        public virtual Customer Customer { get; set; }      // L'auteur du rendez-vous
        public virtual Service Service { get; set; }        // Le service choisis
        public string Comment { get; set; }                 // Un commentaire supplémentaire de la part du client
        [DataType(DataType.DateTime)]
        public DateTime BeginDate { get; set; }             // Date/heure de début du rendez-vous
        
        /***************************Constructeur*******************************/
        public RendezVous() { }
        public RendezVous (Customer _customer, Service _service, string _comment, DateTime _beginDate)
	    {
            Customer = _customer;
            Service = _service;
            Comment= _comment;
            BeginDate= _beginDate;
	    }

        /***************************METHODES*******************************/
        public void PrendreRdv()
        {
        }
        public void DureeRdv()
        {
        }
    }
    
}
