using Agenda.Models.POCO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public class UserDAL : IUserDAL
    {
        private readonly BDDContext bdd;
        public UserDAL(BDDContext context)
        {
            bdd = context;
        }

        public void Add(User user)
        {
            var t = bdd.User.Where(p => p.Email == user.Email).SingleOrDefault();
            if (t != null)
                throw new Exception();

            bdd.User.Add(user);
            bdd.SaveChanges();
        }
        public Task<List<User>> ToListAsync()
        {
            return bdd.User.ToListAsync();
        }
        public void Update(User user)
        {
            bdd.User.Update(user);
            bdd.SaveChanges();
        }

        public bool Exist(User user)
        {
            var t = bdd.User.Where(p => p.Email == user.Email).SingleOrDefault();
            return (t != null) ? true : false;
        }

        public User Get(int id)
        {
            return bdd.User.Where(p => p.ID == id).SingleOrDefault();
        }

        public User Get(string email)
        {
            return bdd.User.Where(p => p.Email == email).SingleOrDefault();
        }

        public List<User> GetAll()
        {
            return bdd.User.ToList();
        }
    }
}
