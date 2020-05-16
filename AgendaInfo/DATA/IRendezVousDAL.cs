using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public interface IRendezVousDAL
    {
        public RendezVous Get(int id);
        public List<RendezVous> GetAll();
        public void Add(RendezVous RendezVous);
        public void Delete(RendezVous RendezVous);
        public void Update(RendezVous RendezVous);
    }
}
