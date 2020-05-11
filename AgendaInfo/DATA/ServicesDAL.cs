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
            bdd.ContextService.Add(srv);
            bdd.SaveChanges();
        }

        public void Delete(Service srv)
        {
            bdd.ContextService.Remove(srv);
            bdd.SaveChanges();
        }

        public Service Get(int id)
        {
            return bdd.ContextService.Where(p => p.ID == id).SingleOrDefault();
        }

        public List<Service> GetAll()
        {
            return bdd.ContextService.ToList();
        }

        public void Update(Service srv)
        {
            bdd.ContextService.Update(srv);
            bdd.SaveChanges();
        }
    }
}
