using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public class DayOffDAL:IDayOffDAL
    {
        private BDDContext bdd;
        public DayOffDAL(BDDContext context)
        {
            bdd = context;
        }
        public DayOff Get(int id)
        {
            return bdd.DayOff.Where(p => p.ID == id).SingleOrDefault();
        }
        public List<DayOff> GetAll()
        {
            return bdd.DayOff.ToList();
        }
        public void Add(DayOff dayoff)
        {
            bdd.DayOff.Add(dayoff);
            bdd.SaveChanges();
        }
        public void Delete(DayOff dayoff)
        {
            bdd.DayOff.Remove(dayoff);
            bdd.SaveChanges();
        }
        public void Update(DayOff dayoff)
        {
            bdd.DayOff.Update(dayoff);
            bdd.SaveChanges();
        }
    }
}
