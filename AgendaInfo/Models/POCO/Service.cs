using AgendaInfo.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class Service
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }         // ID du service
        [Required]
        [Display(Name = "Titre du service :")]
        public string Name { get; set; }    // Nom du service (ce qu'il représente (ex: formatage, réparation clavier, etc..))
        [Required]
        [Range(1,float.MaxValue,ErrorMessage ="Veuillez entrez un prix au-dessus de 0 €")]
        [DataType(DataType.Currency)]
        [Display(Name = "Prix (en euro) du service :")]
        public double Price { get; set; }   // Prix du service
        [Required]
        [Display(Name = "Durée (en minute) service :")]
        [DataType(DataType.Duration)]
        public int Duration { get; set; }   // Durée du service en minute
        /***************************Constructeur*******************************/
        public Service() { }
        public Service(int _ID)
        {
            ID = _ID;
        }
        /***************************METHODES*******************************/

        /*=========================================
        * LoadServiceByID: Charge un service depuis la BDD
        *=========================================*/
        public Service LoadServiceByID(IServicesDAL serviceDAL)
        {
           return serviceDAL.Get(this.ID);
        }

        /*=========================================
         * ToString: Redéfinition du ToString
         *=========================================*/
        public override string ToString()
        {
            return $"Le service {Name} au prix de {Price} € a pour durée {Duration} heures. | {ID}";
        }

    }
}