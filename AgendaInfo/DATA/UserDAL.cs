using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{b
    public class UserDAL : IUserDAL
    {
        private BDDContext bdd;
        public UserDAL(BDDContext context)
        {
            bdd = context;
        }

        public void Add(User user)
        {
            var t = bdd.ContextUser.Where(p => p.Email == user.Email).SingleOrDefault();
            if (t != null)
                throw new Exception();

            bdd.ContextUser.Add(user);
            bdd.SaveChanges();
        }

        public User Get(int id)
        {
            return bdd.ContextUser.Where(p => p.ID == id).SingleOrDefault();
        }

        public User Get(string email)
        {
            return bdd.ContextUser.Where(p => p.Email == email).SingleOrDefault();
        }

        public List<User> GetAll()
        {
            return bdd.ContextUser.ToList();
        }
    }
}
