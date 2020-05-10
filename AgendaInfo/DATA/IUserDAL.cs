using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.Models.DAL
{
    public interface IUserDAL
    {
        public User Get(int id);
        public User Get(string email);
        public List<User> GetAll();
        public void Add(User user);
    }
}
