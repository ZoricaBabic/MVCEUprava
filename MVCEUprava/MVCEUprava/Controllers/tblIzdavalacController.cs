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
    public class tblIzdavalacController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblIzdavalac
        public ActionResult Index()
        {
            return View(db.tblIzdavalacs.ToList());
        }

        // GET: tblIzdavalac/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblIzdavalac tblIzdavalac = db.tblIzdavalacs.Find(id);
            if (tblIzdavalac == null)
            {
                return HttpNotFound();
            }
            return View(tblIzdavalac);
        }

        // GET: tblIzdavalac/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblIzdavalac/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naziv")] tblIzdavalac tblIzdavalac)
        {
            if (ModelState.IsValid)
            {
                db.tblIzdavalacs.Add(tblIzdavalac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblIzdavalac);
        }

        // GET: tblIzdavalac/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblIzdavalac tblIzdavalac = db.tblIzdavalacs.Find(id);
            if (tblIzdavalac == null)
            {
                return HttpNotFound();
            }
            return View(tblIzdavalac);
        }

        // POST: tblIzdavalac/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naziv")] tblIzdavalac tblIzdavalac)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblIzdavalac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblIzdavalac);
        }

        // GET: tblIzdavalac/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblIzdavalac tblIzdavalac = db.tblIzdavalacs.Find(id);
            if (tblIzdavalac == null)
            {
                return HttpNotFound();
            }
            return View(tblIzdavalac);
        }

        // POST: tblIzdavalac/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblIzdavalac tblIzdavalac = db.tblIzdavalacs.Find(id);
            db.tblIzdavalacs.Remove(tblIzdavalac);
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
