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
         * GetAdmin: Retourne l'admin présent dans la BDD
         *=========================================*/
        public Admin GetAdmin(IUserDAL userDAL) => (Admin)userDAL.GetAdmin();

        /*=========================================
         * AddService: Ajoute un service à la Liste
         *=========================================*/
        public void AddService(Service service, IUserDAL userDAL)
        {
            ListServices.Add(service);
            userDAL.Update(this);
        }

        /*=========================================
         * DeleteService: Supprime un service à la Liste
         *=========================================*/
        public void DeleteService(Service service, IUserDAL userDAL)
        {
            ListServices.Remove(service);
            userDAL.Update(this);
        }
    }
}
