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
        public virtual List<Customer> ListCustomers { get; set; }

        /***************************Constructeur*******************************/
        public Admin(int i, string n, string l, DateTime b, string a, int ph, string e, string pa)
            : base(i, n, l, b, a, ph, e, pa) { }


        /***************************METHODES*******************************/

        /*=========================================
         * AddService: Ajoute un service à la Liste
         *=========================================*/
        public void AddService(Service service)
        {
            ListServices.Add(service);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteService: Supprime un service à la Liste
         *=========================================*/
        public void DeleteService(Service service)
        {
            ListServices.Remove(service);
            // TODO: appel de la DAL
        }

        /*=========================================
         * AddCustomer: Ajoute un client à la Liste
         *=========================================*/
        public void AddCustomer(Customer customer)
        {
            ListCustomers.Add(customer);
            // TODO: appel de la DAL
        }

        /*=========================================
         * DeleteCustimer: Supprime un client à la Liste
         *=========================================*/
        public void DeleteCustomer(Customer customer)
        {
            ListCustomers.Remove(customer);
            // TODO: appel de la DAL
        }

    }
}
