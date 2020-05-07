using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class Service
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }         // ID du service
        public string Name { get; set; }    // Nom du service (ce qu'il représente (ex: formatage, réparation clavier, etc..))
        public double Price { get; set; }   // Prix du service
        public int Duration { get; set; }   // Durée du service en minute


        /***************************Constructeur*******************************/
        public Service(string n, double p, int d)
        {
            n = Name;
            p = Price;
            d = Duration;
        }

        /***************************METHODES*******************************/

        /*=========================================
         * ToString: Redéfinition du ToString
         *=========================================*/
        public override string ToString()
        {
            return $"Le service {Name} au prix de {Price} a pour durée {Duration} heures.";
        }

    }
}