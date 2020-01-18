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
    public class tblPotpisController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblPotpis
        public ActionResult Index()
        {
            return View(db.tblPotpis.ToList());
        }

        // GET: tblPotpis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPotpi tblPotpi = db.tblPotpis.Find(id);
            if (tblPotpi == null)
            {
                return HttpNotFound();
            }
            return View(tblPotpi);
        }

        // GET: tblPotpis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblPotpis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Potpis")] tblPotpi tblPotpi)
        {
            if (ModelState.IsValid)
            {
                db.tblPotpis.Add(tblPotpi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblPotpi);
        }

        // GET: tblPotpis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPotpi tblPotpi = db.tblPotpis.Find(id);
            if (tblPotpi == null)
            {
                return HttpNotFound();
            }
            return View(tblPotpi);
        }

        // POST: tblPotpis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Potpis")] tblPotpi tblPotpi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPotpi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPotpi);
        }

        // GET: tblPotpis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPotpi tblPotpi = db.tblPotpis.Find(id);
            if (tblPotpi == null)
            {
                return HttpNotFound();
            }
            return View(tblPotpi);
        }

        // POST: tblPotpis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblPotpi tblPotpi = db.tblPotpis.Find(id);
            db.tblPotpis.Remove(tblPotpi);
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
