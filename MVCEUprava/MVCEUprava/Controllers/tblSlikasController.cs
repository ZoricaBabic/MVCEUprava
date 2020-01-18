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
    public class tblSlikasController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblSlikas
        public ActionResult Index()
        {
            return View(db.tblSlikas.ToList());
        }

        // GET: tblSlikas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSlika tblSlika = db.tblSlikas.Find(id);
            if (tblSlika == null)
            {
                return HttpNotFound();
            }
            return View(tblSlika);
        }

        // GET: tblSlikas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblSlikas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Slika")] tblSlika tblSlika)
        {
            if (ModelState.IsValid)
            {
                db.tblSlikas.Add(tblSlika);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblSlika);
        }

        // GET: tblSlikas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSlika tblSlika = db.tblSlikas.Find(id);
            if (tblSlika == null)
            {
                return HttpNotFound();
            }
            return View(tblSlika);
        }

        // POST: tblSlikas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Slika")] tblSlika tblSlika)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblSlika).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblSlika);
        }

        // GET: tblSlikas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSlika tblSlika = db.tblSlikas.Find(id);
            if (tblSlika == null)
            {
                return HttpNotFound();
            }
            return View(tblSlika);
        }

        // POST: tblSlikas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSlika tblSlika = db.tblSlikas.Find(id);
            db.tblSlikas.Remove(tblSlika);
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
