using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public class RendezVousDAL:IRendezVousDAL
    {
        private BDDContext bdd;
        public RendezVousDAL(BDDContext context)
        {
            bdd = context;
        }
        public RendezVous Get(int id) => bdd.RendezVous.Where(p => p.ID == id).SingleOrDefault();
        public List<RendezVous> GetAll() => bdd.RendezVous.ToList();
        public void Add(RendezVous RendezVous)
        {
            bdd.RendezVous.Add(RendezVous);
            bdd.SaveChanges();
        }
        public void Delete(RendezVous RendezVous)
        {
            bdd.RendezVous.Remove(RendezVous);
            bdd.SaveChanges();
        }
        public void Update(RendezVous RendezVous)
        {
            bdd.RendezVous.Update(RendezVous);
            bdd.SaveChanges();
        }
    }
}
