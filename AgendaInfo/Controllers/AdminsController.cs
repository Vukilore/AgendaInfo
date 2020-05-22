using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agenda.Models.POCO;
using AgendaInfo.DATA;
using Microsoft.AspNetCore.Http;

namespace AgendaInfo.Controllers
{
    public class AdminsController : Controller
    {
        private readonly IUserDAL userDAL;
        public AdminsController(IUserDAL _userDAL) => userDAL = _userDAL;
        public IActionResult Index()
        {
            // 1. On vérifie si l'utilisateur est inscrit
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
            {
                // 1.1 Si il est admin on affiche l'index de l'admin
                if (IsAdmin(HttpContext.Session.GetString("userEmail"))) return View();

                // 1.2 Sinon on affiche l'index du client
                else return RedirectToAction("Index", "Customers");
            }
            else return RedirectToAction("Index", "Home");   // Utilisateur non inscrit, retour à l'accueil 
        }


        /*=========================================
        * IsAdmin: Retourne true si l'email fourni est celui de l'admin
        *=========================================*/
        [NonAction]
        private bool IsAdmin(string email)
        {
            // 1. Création de l'utilisateur temporaire
            User tmpUser = new User(email);
            // 2. Chargement de l'utilisateur grâce à son email
            tmpUser = tmpUser.LoadUserByEmail(userDAL);
            return tmpUser is Admin;
        }
    }
}
