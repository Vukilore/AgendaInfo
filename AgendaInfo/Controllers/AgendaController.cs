using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Agenda.Models.POCO;
using AgendaInfo.DATA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
                     
namespace AgendaInfo.Controllers
{
    public class AgendaController : Controller
    {
        private readonly IRendezVousDAL rdvDAL;
        private readonly IUserDAL userDAL;
        private readonly IDayOffDAL dayOffDAL;
        public AgendaController(IRendezVousDAL _rdvDAL, IUserDAL _userDAL, IDayOffDAL _dayOffDAL)
        {
            rdvDAL = _rdvDAL;
            userDAL = _userDAL;
            dayOffDAL = _dayOffDAL;
        }
        public ActionResult Index(int Week = 0)
        {
            // 1. On défini la semaine à gérer depuis 'aujourd'hui + les semaines à regarder' 
            DateTime WeekToShow = DateTime.Now.AddDays((7 * Week)-1);

            //2. On trouve le lundi de la semaine à gérer
            DateTime MondayOfWeek = WeekToShow.AddDays(-(int)WeekToShow.DayOfWeek + (int)DayOfWeek.Monday);
            ViewBag.MondayOfWeek = MondayOfWeek;

            //3. On crée une liste des RDV de la semaine à gérer
            List<RendezVous> RdvThisWeek = Agenda.Models.POCO.Agenda.GetInstance().ThisWeekRDV(MondayOfWeek, rdvDAL);
                                                                       
            //4. On crée une liste des congés de la semaine à gérer
            ViewBag.DaysOffThisWeek = Agenda.Models.POCO.Agenda.GetInstance().ThisWeekDayOff(MondayOfWeek, dayOffDAL);

            //5. On défini la semaine courrante et le numéro de la semaine dans l'année
            ViewBag.CurrentWeek = Week;
            ViewBag.WeekNumber = GetWeekNumber(MondayOfWeek);     

            //6. On défini si l'utilisateur est l'admin ou non et on le stock dans une Viewbag
            ViewBag.IsAdmin = IsAdmin(HttpContext.Session.GetString("userEmail"));
            User tmpCustomer = new User(HttpContext.Session.GetString("userEmail"));
            ViewBag.Customer = (User)tmpCustomer.LoadUserByEmail(userDAL);
            return View(RdvThisWeek);
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

        /*=========================================
        * GetWeekNumber: Retourne le nombre de semaine de l'année écoulée à la date indiquée
        *=========================================*/
        [NonAction]
        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
        
    }
}