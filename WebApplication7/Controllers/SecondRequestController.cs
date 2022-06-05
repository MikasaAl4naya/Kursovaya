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
    public class SecondRequestController : Controller
    {
        private OlegEntities db = new OlegEntities();
        public ActionResult Index()
        {

            ViewBag.Year_of_receipt = new SelectList(db.Group_2, "Year_of_receipt", "Year_of_receipt");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Year_of_receipt)
        {
            return Redirect("SecondRequest/Result?Year_of_receipt=" + Year_of_receipt);
        }

         public ActionResult Result (int Year_of_receipt, int pg = 1)
        {
            SqlParameter parameter = new SqlParameter("@Year", Year_of_receipt);
            List<SeconD2_Result> requests = db.Database.SqlQuery<SeconD2_Result>("SeconD2 @Year", parameter).ToList();
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