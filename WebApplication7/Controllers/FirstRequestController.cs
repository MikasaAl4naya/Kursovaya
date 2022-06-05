using WebApplication7.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication7.Controllers
{
    public class FirstRequestController : Controller
    {
        private OlegEntities db = new OlegEntities();
        public ActionResult Index()
        {
            ViewBag.Cipher = new SelectList(db.Group_2, "Cipher", "Cipher");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Number, string Cipher)
        {
            try

            {
                int num;
                bool isNum = int.TryParse(Number,out num);
                if (Number == "" || (!isNum) ) throw new Exception("Неверное значение");
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View("Error");
            }
            return Redirect("FirstRequest/Result?number=" + Number + "&group=" + Cipher);
        }

        public ActionResult Result(int number, string group, int pg = 1)
        {
            
            SqlParameter parameter = new SqlParameter("@Number", number);
            SqlParameter parameter2= new SqlParameter("@group", group);
            List<FIRST_Result> requests = db.Database.SqlQuery<FIRST_Result>("FIRST @Number, @group", parameter, parameter2).ToList();
            const int pageSize = 50;
            if (pg < 1)
                pg = 1;

            int rescCount = requests.Count();
            var pages = new Pager(rescCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = requests.Skip(recSkip).Take(pages.PageSize).ToList();
            this.ViewBag.Pager = pages;

            return View(data);
        }

    }
}