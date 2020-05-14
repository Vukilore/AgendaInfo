using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public class ServicesDAL : IServicesDAL
    {
        private BDDContext bdd;
        public ServicesDAL (BDDContext context)
        {
            bdd = context;
        }
        public void Add(Service srv)
        {
            bdd.Service.Add(srv);
            bdd.SaveChanges();
        }

        public void Delete(Service srv)
        {
            bdd.Service.Remove(srv);
            bdd.SaveChanges();
        }

        public Service Get(int id)
        {
            return bdd.Service.Where(p => p.ID == id).SingleOrDefault();
        }

        public List<Service> GetAll()
        {
            return bdd.Service.ToList();
        }

        public void Update(Service srv)
        {
            bdd.Service.Update(srv);
            bdd.SaveChanges();
        }
    }
}
