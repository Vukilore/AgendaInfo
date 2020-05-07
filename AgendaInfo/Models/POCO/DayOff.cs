using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class DayOff
    {

        /***************************PROPRIETES*******************************/
        public int ID { get; set; }                 // ID du congé
        public DateTime StartDate { get; set; }     // Date/Heure du début du congé
        public DateTime EndDate { get; set; }       // Date/Heure de fin du congé
        public string Reason { get; set; }          // Raison personel du congé

        /***************************Constructeur*******************************/
        public DayOff (DateTime start, DateTime end, string r)
        {
            StartDate = start;
            EndDate = end;
            Reason = r;
        }

        /***************************METHODES*******************************/
    }
}
