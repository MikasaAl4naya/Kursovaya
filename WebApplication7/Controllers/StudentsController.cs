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
    public class StudentsController : Controller
    {
        private OlegEntities db = new OlegEntities();

        // GET: Students
        public ActionResult Index(int pg = 1)
        {
            //var entrances = db.Entrances.Include(e => e.Product).Include(e => e.Warehouse);
            List<Student> entrances = db.Students.ToList();

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

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Group_2_Cipher = new SelectList(db.Group_2, "Cipher", "Cipher");
            return View();
        }

        // POST: Students/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordBook,Group_2_Cipher,FatherSalary,MotherSalary,FullName,Family")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    db.addStudent(student.RecordBook, student.Group_2_Cipher, student.FatherSalary, student.MotherSalary, student.FullName, student.Family);
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
                ModelState.AddModelError("", "Ошибка! Такой студент уже существует");
            }

            ViewBag.Group_2_Cipher = new SelectList(db.Group_2, "Cipher", "Cipher", student.Group_2_Cipher);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Group_2_Cipher = new SelectList(db.Group_2, "Cipher", "Cipher", student.Group_2_Cipher);
            return View(student);
        }

        // POST: Students/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordBook,Group_2_Cipher,FatherSalary,MotherSalary,FullName,Family,NewRecordBook")] Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //db.updateStudent(student.RecordBook, student.Group_2_Cipher, student.FatherSalary, student.MotherSalary, student.FullName, student.Family, student.RecordBook)
                    //;
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Ошибка! Какая то ошибка короче");
                }
            }
            ViewBag.Group_2_Cipher = new SelectList(db.Group_2, "Cipher", "Cipher", student.Group_2_Cipher);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            try
            {
               
                db.Students.Remove(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка! Объект уже используется в другой таблице!");
            }
            return View(student);
            
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
