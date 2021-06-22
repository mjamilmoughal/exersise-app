using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using App.DataTransferObjects;
using App.Models;

namespace App.Controllers
{
    [Authorize]
    public class StepsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Steps
        private string GetIdByEmail(string email)
        {
            return db.Users.Where(x => x.UserName == email).Select(r => r.Id).FirstOrDefault();
        }

        public ActionResult Index()
        {
            var data = (from x in db.Steps
                        join y in db.Users
                        on x.CreatedBy equals y.Id
                        select new StepDto
                        {
                            CreatedBy = y.FullName,
                            ImagePath = x.ImagePath,
                            StepName = x.StepName,
                            StepsCount = x.StepsCount,
                            Id = x.Id,
                            CreatedDate = x.CreatedDate
                        }).ToList();
            return View(data);
        }

        // GET: Steps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Step step = db.Steps.Find(id);
            if (step == null)
            {
                return HttpNotFound();
            }
            return View(step);
        }

        // GET: Steps/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StepDto step)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(step.Image.FileName);
                var ext = Path.GetExtension(step.Image.FileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string myfile = name + "_" + Guid.NewGuid() + ext;
                var path = Path.Combine(Server.MapPath("~/Images"), myfile);

                step.Image.SaveAs(path);

                var loggedInName = GetIdByEmail(User.Identity.Name);

                var stp = new Step
                {
                    CreatedBy = loggedInName,
                    CreatedDate = DateTime.Now,
                    ImagePath = "/Images/" + myfile,
                    StepName = step.StepName,
                    StepsCount = step.StepsCount
                };

                db.Steps.Add(stp);
                db.SaveChanges();
                return Json(new { status = true }, JsonRequestBehavior.AllowGet);
            }

            return View(step);
        }

        // GET: Steps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Step step = db.Steps.Find(id);
            if (step == null)
            {
                return HttpNotFound();
            }
            return View(step);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Step step)
        {
            if (ModelState.IsValid)
            {
                db.Entry(step).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(step);
        }

        // GET: Steps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Step step = db.Steps.Find(id);
            if (step == null)
            {
                return HttpNotFound();
            }
            step.CreatedBy = db.Users.Where(x => x.Id == step.CreatedBy).Select(r => r.FullName).FirstOrDefault();
            return View(step);
        }

        // POST: Steps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Step step = db.Steps.Find(id);
            db.Steps.Remove(step);
            db.SaveChanges();
            return RedirectToAction("Index");
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
