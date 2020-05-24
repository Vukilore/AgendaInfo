using AgendaInfo.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Models.POCO
{
    public class User
    {
        /***************************PROPRIETES*******************************/
        public int ID { get; set; }                 // ID de l'utilisateur

        [Required(ErrorMessage = "Veuillez entrer votre nom de famille.")]
        [Display(Name = "Nom de famille : ")] 
        public string Name { get; set; }            // Nom de l'utilisateur

        [Required(ErrorMessage = "Veuillez entrer votre prénom.")]
        [Display(Name = "Prénom : ")]
        public string FirstName { get; set; }       // Prénom de l'utilisateur

        [Required(ErrorMessage = "Veuillez entrer votre date de naissance.")]
        [Display(Name = "Date de naissance : ")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }      // Date de naissance de l'utilisateur

        [Required(ErrorMessage = "Veuillez entrer votre adresse d'habitation.")]
        [Display(Name = "Adresse de location : ")]
        public string Address { get; set; }         // Adresse de l'utilisateur

        [Required(ErrorMessage = "Veuillez entrer votre numéro de téléphone.")]
        [Display(Name = "Numero de téléphone : ")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression("(0|\\+32|0032)[1-9][0-9]{8}",ErrorMessage ="Veuillez entrer un numéro de mobile valide")] // regex pour numéro de gsm    
        public int PhoneNumber { get; set; }        // Numéro de téléphone de l'utilisateur

        [Required(ErrorMessage = "Veuillez entrer votre email.")]
        [Display(Name = "Email : ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }           // Adresse email de l'utilisateur

        [Required(ErrorMessage = "Veuillez entrer votre mot de passe.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe : ")]
        public string Password { get; set; }        // Mot de passe de l'utilisateur

        [Required(ErrorMessage = "Veuillez confirmer votre mot de passe.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirmer mot de passe: ")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        /***************************Constructeur*******************************/
        public User() { }
        public User(string _email) => Email = _email;


        /***************************METHODES*******************************/

        /*=========================================
         * LoadUser: Charge l'utilisateur depuis la BDD
         *=========================================*/
        public User LoadUserByEmail(IUserDAL userDAL) => userDAL.Get(Email);

        /*=========================================
         * ToString: Redéfinition du ToString
         *=========================================*/
        public override string ToString() => $"{Name} {FirstName} | {Email}";
    }
}
