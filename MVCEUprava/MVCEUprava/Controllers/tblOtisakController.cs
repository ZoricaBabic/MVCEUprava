using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCEUprava.Models;

namespace MVCEUprava.Controllers
{
    public class tblOtisakController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblOtisak
        public ActionResult Index()
        {
            return View(db.tblOtisaks.ToList());
        }

        // GET: tblOtisak/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOtisak tblOtisak = db.tblOtisaks.Find(id);
            if (tblOtisak == null)
            {
                return HttpNotFound();
            }
            return View(tblOtisak);
        }

        // GET: tblOtisak/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblOtisak/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Otisak")] tblOtisak tblOtisak)
        {
            if (ModelState.IsValid)
            {
                db.tblOtisaks.Add(tblOtisak);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblOtisak);
        }

        // GET: tblOtisak/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOtisak tblOtisak = db.tblOtisaks.Find(id);
            if (tblOtisak == null)
            {
                return HttpNotFound();
            }
            return View(tblOtisak);
        }

        // POST: tblOtisak/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Otisak")] tblOtisak tblOtisak)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblOtisak).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblOtisak);
        }

        // GET: tblOtisak/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOtisak tblOtisak = db.tblOtisaks.Find(id);
            if (tblOtisak == null)
            {
                return HttpNotFound();
            }
            return View(tblOtisak);
        }

        // POST: tblOtisak/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblOtisak tblOtisak = db.tblOtisaks.Find(id);
            db.tblOtisaks.Remove(tblOtisak);
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
