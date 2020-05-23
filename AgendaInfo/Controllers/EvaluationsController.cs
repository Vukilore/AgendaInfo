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

        public EvaluationsController(IUserDAL _userDAL, IEvalDAL _evalDAL)
        {
            userDAL = _userDAL;
            evalDAL = _evalDAL;
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
            return View();
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

        public IActionResult Create(int idRendezVous)
        {
            // 1. Si l'utilisateur est connecté 
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
            {
                //1.1 L'admin ne peut pas ajouter d'evaluation, on affiche donc la vue cheat 
                if (IsAdmin(HttpContext.Session.GetString("userEmail"))) return View("Cheat");
                else
                {
                    // 1.2 Création de l'utilisateur temporaire
                    User currentUser = new User(HttpContext.Session.GetString("userEmail"));
                    currentUser.LoadUserByEmail(userDAL);
                    Customer tmpCustomer = (Customer)currentUser;

                    // 1.3 Création du rendez-vous temportaire
                    RendezVous tmpRDV = tmpCustomer.ListRendezVous.Find(r => r.ID == idRendezVous);
                    
                    // 1.4 Si l'utilisateur n'a pas l'ID dans sa liste, ce n'est pas son rdv ou il n'existe pas
                    if(tmpRDV == null)   return NotFound();

                    // 1.5 On insert le rendez-vous dans le ViewData
                    ViewData["RendezVous"] = tmpRDV;
     
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ID,Rate,Comment, RendezVous")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                ViewData["Evaluation"] = evaluation;
                View("Succeed");
            }
            return View(evaluation);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Evaluation evaluation;
            Agenda.Models.POCO.Agenda.GetInstance().Update(evalDAL);
            evaluation = Agenda.Models.POCO.Agenda.GetInstance().GetEvaluation((int)id);

            if (evaluation == null)   return NotFound();

            return View(evaluation);
        }
   /*
        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Rate,Comment")] Evaluation evaluation)
        {
            if (id != evaluation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluationExists(evaluation.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(evaluation);
        }

        // GET: Evaluations/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .FirstOrDefaultAsync(m => m.ID == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var evaluation = await _context.Evaluation.FindAsync(id);
            _context.Evaluation.Remove(evaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluationExists(int id)
        {
            return _context.Evaluation.Any(e => e.ID == id);
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
