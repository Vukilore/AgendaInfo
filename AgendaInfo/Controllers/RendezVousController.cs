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
    public class RendezVousController : Controller
    {
        private readonly IRendezVousDAL rdvDAL;
        private readonly IUserDAL userDAL;
        private readonly IServicesDAL serviceDAL;
        private readonly IDayOffDAL dayOffDAL;
        
        public RendezVousController(IRendezVousDAL _rdvDAL, IUserDAL _userDAL, IServicesDAL _servicesDAL, IDayOffDAL _dayOffDAL)
        {
            rdvDAL = _rdvDAL;
            userDAL = _userDAL;
            serviceDAL = _servicesDAL;
            dayOffDAL = _dayOffDAL;
        }

        // GET: RendezVous
        public IActionResult Index()
        {
            //1. On récupère l'utilisateur courant
            User currentUser = new User(HttpContext.Session.GetString("userEmail"));
            currentUser = currentUser.LoadUserByEmail(userDAL);
            if (currentUser is Admin)
            {
                Agenda.Models.POCO.Agenda.GetInstance().Update(rdvDAL);
                return View(Agenda.Models.POCO.Agenda.GetInstance().ListRendezVous);
            }
            else
            {
                Customer tmpCustomer = (Customer)currentUser;
                return View(tmpCustomer.ListRendezVous);
            }
            
        }

        // GET: RendezVous/Create
        public IActionResult Create(DateTime time)
        {
            //1. On récupère l'utilisateur courant
            User currentUser = new User(HttpContext.Session.GetString("userEmail"));
            currentUser = currentUser.LoadUserByEmail(userDAL);

            //2. On Stock la date de rendez vous
            ViewBag.ReservedDate = time;

            //2. On récupère les utilisateurs
            List<User> listUser = new List<User>();
            listUser = userDAL.GetAll();

            //3. On récupère les services
            Admin tmpUser = (Admin)listUser.Find(s => s is Admin);
            ViewBag.ListServices = tmpUser.ListServices;

            //3. Si il est admin on stock la liste
            if (currentUser is Admin)
            {
                // 3.1 On enlève l'admin de la liste (forcément currentUser dans ce cas)
                listUser.Remove(currentUser);
                // 3.2 On stock la liste des utilisateurs dans un Viewbag sous forme de SelectList
                ViewBag.ListCustomers = listUser;
                // 3.3 On déclarera qu'il s'agit d'une vue pour l'admin (todo vue partiel)
                ViewBag.IsAdmin = true;
            }
            else
            {
                ViewBag.IsAdmin = false;
                ViewBag.Customer = (Customer)currentUser;
            }  
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Comment,BeginDate, Customer, Service")] RendezVous rendezVous)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(Request.Form["Customer"]) && !string.IsNullOrEmpty(Request.Form["Service"]))
            {
                // HACK: Vu que le binding ne fonctionne pas pour le moment, on utilise un petit hack pour contourner ceci:
                // On utilise la méthode Request.Form pour rechercher la value Customer du formulaire
                // Au lieu d'obtenir l'objet en lui même, on va chercher son ToString
                // On va couper le string pour ne garder que l'email de l'utilisateur (qui se situe après le mark  '|' )
                // Et charger dans la bdd le client correspondant à l'email
                // TODO: Fixer le bind pour être plus optimisé
                User tmpCustomer = new User(Request.Form["Customer"].ToString().Remove(0, Request.Form["Customer"].ToString().IndexOf("|") + 2));
                rendezVous.Customer = (Customer)tmpCustomer.LoadUserByEmail(userDAL);

                // HACK: on utilise le même hack pour les services :
                Service tmpService = new Service(Convert.ToInt32(Request.Form["Service"].ToString().Remove(0, Request.Form["Service"].ToString().IndexOf("|") + 2)));
                rendezVous.Service = tmpService.LoadServiceByID(serviceDAL);
                
                // Si le service ne dépasse pas 17h
                int tmpHour = rendezVous.BeginDate.Hour;
                if (tmpHour + rendezVous.Service.Duration <= 17)
                {
                    // Si il n'y a pas de rendez-vous prévu durant le temps du service
                    if(Agenda.Models.POCO.Agenda.GetInstance().FreeOfRendezVous(rendezVous.BeginDate, rendezVous.BeginDate.AddHours(rendezVous.Service.Duration), rdvDAL)) 
                    {
                        //si il n'y a pas de congés prévu durant le temps du service
                        if (Agenda.Models.POCO.Agenda.GetInstance().FreeOfDayOff(rendezVous.BeginDate, rendezVous.BeginDate.AddHours(rendezVous.Service.Duration), dayOffDAL))
                        {
                            if (rendezVous.Comment == null) rendezVous.Comment = "Aucune";
                            // On sauvegarde le rendez-vous
                            Agenda.Models.POCO.Agenda.GetInstance().AddRendezVous(rendezVous, rdvDAL);
                            ViewBag.rendezvous = rendezVous;
                            return View("Succeed");
                        } else return View("ErrorRDV");
                    } else return View("ErrorRDV");
                } else return View("ErrorRDV");
            }
            return View(rendezVous);
        }

        public IActionResult Delete(int? id)
        {             
            // 1. Si l'ID fourni en paramètre est null on retourne notfound
            if (id == null)  return NotFound();

            // 2. On charge la liste des RDV de l'agenda 
            Agenda.Models.POCO.Agenda.GetInstance().Update(rdvDAL);
            RendezVous tmpRDV = new RendezVous();

            // 3. Si il a trouvé le rendez-vous depuis l'ID
            tmpRDV = Agenda.Models.POCO.Agenda.GetInstance().ListRendezVous.Find(r => r.ID == id);

            // 5. Sinon on retourne notfound
            if (tmpRDV == null) NotFound();

            // 5. Si l'utilisateur n'est pas admin
            if (!IsAdmin(HttpContext.Session.GetString("userEmail")))
            {
                // 5.1 On récupère l'utilisateur courant
                User currentUser = new User(HttpContext.Session.GetString("userEmail"));
                currentUser = currentUser.LoadUserByEmail(userDAL);
                // 5.2 Si ce n'est pas le rendez-vous de l'utilisateur
                if (tmpRDV.Customer != currentUser)
                    return RedirectToAction("Index", "Agenda");
            }
            else ViewBag.IsAdmin = true;
            return View(tmpRDV);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // 1. Si l'utilisateur courant est pas admin, on le redirige
            if (!IsAdmin(HttpContext.Session.GetString("userEmail"))) Redirect("../Customer/Index");
           
            // 2. On charge la liste des RDV de l'agenda 
            Agenda.Models.POCO.Agenda.GetInstance().Update(rdvDAL);
            RendezVous tmpRDV = new RendezVous();

            // 3. Si il a trouvé le rendez-vous depuis l'ID
            tmpRDV = Agenda.Models.POCO.Agenda.GetInstance().ListRendezVous.Find(r => r.ID == id);

            // 4. On supprime le RDV
            Agenda.Models.POCO.Agenda.GetInstance().DeleteRendezVous(tmpRDV, rdvDAL);
            return RedirectToAction("Index", "Agenda");
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
