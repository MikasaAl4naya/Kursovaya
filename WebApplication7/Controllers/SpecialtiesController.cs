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
    public class SpecialtiesController : Controller
    {
        private OlegEntities db = new OlegEntities();

        // GET: Specialties
        public ActionResult Index(int pg =1)
        {
            List<Specialty> entrances = db.Specialties.ToList();

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

        // GET: Specialties/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialty specialty = db.Specialties.Find(id);
            if (specialty == null)
            {
                return HttpNotFound();
            }
            return View(specialty);
        }

        // GET: Specialties/Create
        public ActionResult Create()
        {
            ViewBag.Faculty_Title = new SelectList(db.Faculties, "Title", "Title");
            return View();
        }

        // POST: Specialties/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Faculty_Title,Cipher,Qualification")] Specialty specialty)
        {
            
                if (ModelState.IsValid)
                {
                try
                {
                    db.addSpeciality(specialty.Title, specialty.Faculty_Title, specialty.Cipher, specialty.Qualification);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException == null)
                    {
                        ViewBag.error = ex.Message;
                    }
                    else
                    {
                        ViewBag.error = ex.InnerException.Message;
                    }
                    ModelState.AddModelError("", "Специальность не может быть добавлена");
                }
                return View(specialty);
            }
            ViewBag.Faculty_Title = new SelectList(db.Faculties, "Title", "Title",specialty.Faculty_Title);
            return View(specialty);
        }

        // GET: Specialties/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialty specialty = db.Specialties.Find(id);
            if (specialty == null)
            {
                return HttpNotFound();
            }
            ViewBag.Faculty_Title = new SelectList(db.Faculties, "Title", "Title", specialty.Faculty_Title);
            return View(specialty);
        }

        // POST: Specialties/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,Faculty_Title,Cipher,Qualification")] Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(specialty).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    
                    ModelState.AddModelError("", "Специальность не может быть изменена");
                }
            }
            ViewBag.Faculty_Title = new SelectList(db.Faculties, "Title", "Dean_FN", specialty.Faculty_Title);
            return View(specialty);
        }

        // GET: Specialties/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialty specialty = db.Specialties.Find(id);
            if (specialty == null)
            {
                return HttpNotFound();
            }
            return View(specialty);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Specialty specialty = db.Specialties.Find(id);
            try
            {
                db.Specialties.Remove(specialty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Ошибка! Объект уже используется в другой таблице!");
            }
            return View(specialty);
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
