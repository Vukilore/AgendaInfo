using Agenda.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaInfo.DATA
{
    public interface IUserDAL
    {
        
        public User Get(int id);
        public User Get(string email);
        public List<User> GetAll();
        public bool Exist(User user);
        public void Update(User user);
        public void Add(User user);
        public List<Customer> GetAllCustomers();
        public User GetAdmin();
    }
}
