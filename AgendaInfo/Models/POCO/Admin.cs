using AgendaInfo.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class Admin : User
    {
        /***************************PROPRIETES*******************************/
        public virtual List<Service> ListServices { get; set; }

        /***************************Constructeur*******************************/
        public Admin() { }
        public Admin(string _email):base(_email) { }
        
        /***************************METHODES*******************************/

        /*=========================================
         * AddService: Ajoute un service à la Liste
         *=========================================*/
        public void AddService(Service service, IServicesDAL serviceDAL)
        {
            ListServices.Add(service);
            serviceDAL.Add(service); 
        }

        /*=========================================
         * DeleteService: Supprime un service à la Liste
         *=========================================*/
        public void DeleteService(Service service, IServicesDAL serviceDAL)
        {
            ListServices.Remove(service);
            serviceDAL.Delete(service);
        }
    }
}
