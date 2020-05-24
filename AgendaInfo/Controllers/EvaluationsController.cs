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
    public class EvaluationsController : Controller
    {
        private readonly IUserDAL userDAL;
        private readonly IEvalDAL evalDAL;
        private readonly IRendezVousDAL rdvDAL;

        public EvaluationsController(IUserDAL _userDAL, IEvalDAL _evalDAL, IRendezVousDAL _rdvDAL)
        {
            userDAL = _userDAL;
            evalDAL = _evalDAL;
            rdvDAL = _rdvDAL;
        }

        public IActionResult Index()
        {
            if (IsAdmin(HttpContext.Session.GetString("userEmail")))
                ViewBag.IsAdmin = true;

            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            return View(Agenda.Models.POCO.Agenda.GetInstance().ListEvaluations);
        }

        public IActionResult ListEvaluations()
        {
            Agenda.Models.POCO.Agenda.GetInstance().Update(userDAL);
            Customer tmpCustomer = Agenda.Models.POCO.Agenda.GetInstance().GetCustomer(HttpContext.Session.GetString("userEmail"));
            return View(tmpCustomer.ListEvaluation);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            Evaluation evaluation;
            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            evaluation = Agenda.Models.POCO.Agenda.GetInstance().GetEvaluation((int)id);

            if (evaluation == null)return NotFound();
            return View(evaluation);
        }

        public IActionResult Create(int? id)
        {
            // 1. Si l'utilisateur est connecté 
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
            {
                //1.1 L'admin ne peut pas ajouter d'evaluation, on affiche donc la vue cheat 
                if (IsAdmin(HttpContext.Session.GetString("userEmail"))) return View("Cheat");
                else
                {
                    // 1.2 Chargement du client
                    Agenda.Models.POCO.Agenda.GetInstance().Update(userDAL);
                    Customer tmpCustomer = Agenda.Models.POCO.Agenda.GetInstance().GetCustomer(HttpContext.Session.GetString("userEmail"));        

                    // 1.3 Création du rendez-vous temportaire
                    RendezVous tmpRDV = tmpCustomer.ListRendezVous.Find(r => r.ID == (int)id);
                    
                    // 1.4 Si l'utilisateur n'a pas l'ID dans sa liste, ce n'est pas son rdv ou il n'existe pas
                    if(tmpRDV == null)   return NotFound();

                    if (tmpRDV.BeginDate >= DateTime.Now) return View("ErrorEval");

                    int evalExist = Agenda.Models.POCO.Agenda.GetInstance().RendezVousRated(tmpRDV, evalDAL);

                    if (evalExist != -1) return RedirectToAction("Details", "Evaluations", new { id = evalExist });

                    // 1.5 On insert le rendez-vous dans le ViewData
                    ViewData["RendezVous"] = tmpRDV;
                                                                        
                    return View();  
     
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Rate,Comment, RendezVous")] Evaluation evaluation)
        {
            //1. HACK: Comme pour la prise de rdv on utilise l'ID du rdv depuis le form avec le request.form
            //TODO: trouver comment faire fonctionner le bind sur l'objet
            int rendezVousID = Convert.ToInt32(Request.Form["RendezVous"]);
           
            // 2. On met à jour la liste de l'agenda pour récupérer le rdv depuis l'ID 
            Agenda.Models.POCO.Agenda.GetInstance().Update(rdvDAL);
            evaluation.RendezVous = Agenda.Models.POCO.Agenda.GetInstance().GetRendezVous(rendezVousID);

            
            // 3. On ajoute l'utilisateur courant
            Agenda.Models.POCO.Agenda.GetInstance().Update(userDAL);
            Customer tmpCustomer = Agenda.Models.POCO.Agenda.GetInstance().GetCustomer(HttpContext.Session.GetString("userEmail"));

            // 4. On sauvegarde l'evaluation
            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            Agenda.Models.POCO.Agenda.GetInstance().AddEvaluation(evaluation, userDAL);

            ViewData["Evaluation"] = evaluation;
            return View("Succeed");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Evaluation evaluation;
            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            evaluation = Agenda.Models.POCO.Agenda.GetInstance().GetEvaluation((int)id);

            if (evaluation == null)   return NotFound();

            Agenda.Models.POCO.Agenda.GetInstance().Update(userDAL);
            Customer tmpCustomer = Agenda.Models.POCO.Agenda.GetInstance().GetCustomer(HttpContext.Session.GetString("userEmail"));

            if (evaluation.RendezVous.Customer != tmpCustomer) return NotFound();

            return View(evaluation);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("ID, Rate,Comment")] Evaluation evaluation)
        {
            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            var e = evaluation;
            Agenda.Models.POCO.Agenda.GetInstance().EditEvaluation(evaluation, userDAL);
            return View("Succeed");
        }
        
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            Evaluation evaluation = Agenda.Models.POCO.Agenda.GetInstance().GetEvaluation((int)id);

            if (evaluation == null) return NotFound();

            Customer tmpCustomer = Agenda.Models.POCO.Agenda.GetInstance().GetCustomer(HttpContext.Session.GetString("userEmail"));

            if (evaluation.RendezVous.Customer != tmpCustomer) return NotFound();

            return View(evaluation);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            Evaluation evaluation = Agenda.Models.POCO.Agenda.GetInstance().GetEvaluation((int)id);
            Agenda.Models.POCO.Agenda.GetInstance().DeleteEvaluation(evaluation, userDAL, evalDAL);
            return RedirectToAction(nameof(Index));
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
