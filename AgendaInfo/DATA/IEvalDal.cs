using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public interface IEvalDAL
    {
        public Evaluation Get(int id);
        public Evaluation Get(RendezVous rdv);
        public List<Evaluation> GetAll();
        public void Add(Evaluation eval);
        public void Delete(Evaluation eval);
        public void Update(Evaluation eval);
    }
}
