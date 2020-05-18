using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public interface IServicesDAL
    {
        public Service Get(int id);
        public List<Service> All { get; }
        public void Add(Service srv);
        public void Delete(Service srv);
        public void Update(Service srv);

    }
}
