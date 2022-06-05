using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class GroupController : Controller
    {
        private OlegEntities db = new OlegEntities();

        // GET: Group
        public ActionResult Index(int pg = 1)
        {
            //var entrances = db.Entrances.Include(e => e.Product).Include(e => e.Warehouse);
            List<Group_2> entrances = db.Group_2.ToList();

            const int pageSize = 15;
            if (pg < 1)
                pg = 1;

            int rescCount = entrances.Count();
            var pager = new Pager(rescCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = entrances.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
        }

        // GET: Group/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group_2 group_2 = db.Group_2.Find(id);
            if (group_2 == null)
            {
                return HttpNotFound();
            }
            return View(group_2);
        }

        // GET: Group/Create
        public ActionResult Create()
        {
            ViewBag.Specialty_Title = new SelectList(db.Specialties, "Title", "Title");
            return View();
        }

        // POST: Group/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cipher,Specialty_Title,Year_of_receipt")] Group_2 group_2)
        {
            try {

                if (ModelState.IsValid)
                {
                    // db.Group_2.Add(group_2);
                    db.addGroup(group_2.Cipher, group_2.Specialty_Title, group_2.Year_of_receipt);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException == null)
                {
                    ViewBag.error = ex.Message;
                }
                else
                {
                    ViewBag.error = ex.InnerException.Message;
                }
                ModelState.AddModelError("", "Ошибка! Уже существует такая группа");
            }

            ViewBag.Specialty_Title = new SelectList(db.Specialties, "Title", "Faculty_Title", group_2.Specialty_Title);
            return View(group_2);
        }

        // GET: Group/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group_2 group_2 = db.Group_2.Find(id);
            if (group_2 == null)
            {
                return HttpNotFound();
            }
            ViewBag.Specialty_Title = new SelectList(db.Specialties, "Title", "Faculty_Title", group_2.Specialty_Title);
            return View(group_2);
        }

        // POST: Group/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cipher,Specialty_Title,Year_of_receipt")] Group_2 group_2)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(group_2).State = EntityState.Modified;
                    //db.updateGroup(group_2.Specialty_Title, group_2.Cipher, group_2.Year_of_receipt, group_2.Cipher);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Ошибка! Некорректный тип данных");
            }
            ViewBag.Specialty_Title = new SelectList(db.Specialties, "Title", "Faculty_Title", group_2.Specialty_Title);
            return View(group_2);
        }

        // GET: Group/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group_2 group_2 = db.Group_2.Find(id);
            if (group_2 == null)
            {
                return HttpNotFound();
            }
            return View(group_2);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Group_2 group_2 = db.Group_2.Find(id);
            try
            {
                
                db.Group_2.Remove(group_2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка! Объект уже используется в другой таблице!");
            }
            return View(group_2) ;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
