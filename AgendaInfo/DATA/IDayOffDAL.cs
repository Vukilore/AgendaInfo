using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public interface IDayOffDAL
    {
        public DayOff Get(int id);
        public List<DayOff> All { get; }
        public void Add(DayOff dayoff);
        public void Delete(DayOff dayoff);
        public void Update(DayOff dayoff);
    }
}
