using AgendaInfo.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class User
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }                 // ID de l'utilisateur
        public string Name { get; set; }            // Nom de l'utilisateur
        public string FirstName { get; set; }       // Prénom de l'utilisateur
        public DateTime Birthday { get; set; }      // Date de naissance de l'utilisateur
        public string Address { get; set; }         // Adresse de l'utilisateur
        public int PhoneNumber { get; set; }        // Numéro de téléphone de l'utilisateur
        public string Email { get; set; }           // Adresse email de l'utilisateur
        public string Password { get; set; }        // Mot de passe de l'utilisateur

        /***************************Constructeur*******************************/
        public User (int i, string n, string l, DateTime b, string a, int ph, string e, string pa)
        {
            i = ID;
            n = Name;
            l = FirstName;
            b = Birthday;
            a = Address;
            ph = PhoneNumber;
            e = Email;
            pa = Password;
        }

        /***************************METHODES*******************************/

        /*=========================================
         * LoadUser: Charge l'utilisateur depuis la BDD
         *=========================================*/
        public void LoadUserByEmail(IUserDAL userDAL) { 
                this = userDAL.Get(Email);
        } 
        // public User LoadUserByID(IUser userDAL) { return 0; } // TODO: A compléter

    }
}
