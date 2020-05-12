using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public class EvalDAL:IEvalDAL
    {
        private BDDContext bdd;
        public EvalDAL(BDDContext context)
        {
            bdd = context;
        }
        public Evaluation Get(int id)
        {
            return bdd.ContextEvaluation.Where(p => p.ID == id).SingleOrDefault();

        }
        public Evaluation Get(RendezVous rdv)
        {
            return bdd.ContextEvaluation.Where(p => p.RendezVous == rdv).SingleOrDefault();
        }
        
        public List<Evaluation> GetAll()
        {
            return bdd.ContextEvaluation.ToList();
        }
        public void Add(Evaluation eval)
        {
            bdd.ContextEvaluation.Add(eval);
            bdd.SaveChanges();
        }
        public void Delete(Evaluation eval)
        {
            bdd.ContextEvaluation.Remove(eval);
            bdd.SaveChanges();
        }
        public void Update(Evaluation eval)
        {
            bdd.ContextEvaluation.Update(eval);
            bdd.SaveChanges();
        }
    }
}