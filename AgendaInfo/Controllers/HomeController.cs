using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AgendaInfo.Models;
using Agenda.Models.POCO;
using Microsoft.AspNetCore.Http;
using AgendaInfo.DATA;

namespace AgendaInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserDAL userDAL;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IUserDAL _userDAL)
        {
            userDAL = _userDAL;
            _logger = logger;
        }

        public IActionResult Index() 
        {
            if(!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
            {// L'utilisateur est connecté
                User user = new User(HttpContext.Session.GetString("userEmail"));
                user.LoadUserByEmail(userDAL);
                if (user is Admin) return Redirect("../Admin/Index");
                else return Redirect("../Customers/Index");
            }
            return View("Index");
        }

        /////////////////////////////////////////////////////////////////////////////////
        ///                               LOGIN                                       ///
        /////////////////////////////////////////////////////////////////////////////////
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail"))) RedirectToAction(nameof(Index));
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user) // Réponse du formulaire de connexion
        {
            // 1. Création de l'utilisateur temporaire
            User tmpUser = new User(user.Email);

            // 2. Chargement de l'utilisateur grâce à son email
            tmpUser = tmpUser.LoadUserByEmail(userDAL);    

            // 3. Vérification si l'utilisateur existe
            if(tmpUser == null) // Si il n'a pas trouvé l'email dans la BDD
            {
                ViewBag.Message = "Cet email n'existe pas dans notre base de donnée !";
                return View("Login"); // Redirection pour afficher l'erreur
            }
            else // L'email existe
            {
                // 3.1 Vérification du mot de passe
                if (user.Password == tmpUser.Password) // Le mot de passe correspond bien à celui de la BDD
                {
                    HttpContext.Session.SetString("userEmail", tmpUser.Email);
                    if (tmpUser is Admin) return Redirect("../Admin/Index"); 
                    else return Redirect("../Customers/Index/");
                }
                else
                {
                    ViewBag.Message = "Le mot de passe que vous avez entré est incorrecte.";
                    return View("Login");
                }
            }
        }

        public ActionResult Disconnect() // Le bouton de deconnexion
        {
            HttpContext.Session.Clear(); // On nettoie toutes les variables de session
            return Redirect("Index"); // On redirige au point de départ (connexion/inscription)
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
