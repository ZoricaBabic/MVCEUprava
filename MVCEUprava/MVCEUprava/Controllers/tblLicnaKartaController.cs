using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCEUprava.App_Start;
using MVCEUprava.Models;

namespace MVCEUprava.Controllers
{
    public class tblLicnaKartaController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblLicnaKarta
        [CustomAuthorize]
        public ActionResult Index()
        {
            var tblLicnaKartas = db.tblLicnaKartas.Include(t => t.tblKorisnikAplikacije).Include(t => t.tblKorisnikLicneKarte);
            return View(tblLicnaKartas.ToList());
        }

        // GET: tblLicnaKarta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLicnaKarta tblLicnaKarta = db.tblLicnaKartas.Find(id);
            if (tblLicnaKarta == null)
            {
                return HttpNotFound();
            }
            return View(tblLicnaKarta);
        }

        // GET: tblLicnaKarta/Create
        public ActionResult Create()
        {
            ViewBag.KorisnikAplikacije = new SelectList(db.tblKorisnikAplikacijes, "Id", "Ime");
            ViewBag.KorisnikLicneKarte = new SelectList(db.tblKorisnikLicneKartes, "Id", "Ime");
            return View();
        }

        // POST: tblLicnaKarta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RegistarskiBroj,DatumIzdavanja,DatumIsteka,KorisnikAplikacije,KorisnikLicneKarte")] tblLicnaKarta tblLicnaKarta)
        {
            if (ModelState.IsValid)
            {
                db.tblLicnaKartas.Add(tblLicnaKarta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KorisnikAplikacije = new SelectList(db.tblKorisnikAplikacijes, "Id", "Ime", tblLicnaKarta.KorisnikAplikacije);
            ViewBag.KorisnikLicneKarte = new SelectList(db.tblKorisnikLicneKartes, "Id", "Ime", tblLicnaKarta.KorisnikLicneKarte);
            return View(tblLicnaKarta);
        }

        // GET: tblLicnaKarta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLicnaKarta tblLicnaKarta = db.tblLicnaKartas.Find(id);
            if (tblLicnaKarta == null)
            {
                return HttpNotFound();
            }
            ViewBag.KorisnikAplikacije = new SelectList(db.tblKorisnikAplikacijes, "Id", "Ime", tblLicnaKarta.KorisnikAplikacije);
            ViewBag.KorisnikLicneKarte = new SelectList(db.tblKorisnikLicneKartes, "Id", "Ime", tblLicnaKarta.KorisnikLicneKarte);
            return View(tblLicnaKarta);
        }

        // POST: tblLicnaKarta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RegistarskiBroj,DatumIzdavanja,DatumIsteka,KorisnikAplikacije,KorisnikLicneKarte")] tblLicnaKarta tblLicnaKarta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLicnaKarta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KorisnikAplikacije = new SelectList(db.tblKorisnikAplikacijes, "Id", "Ime", tblLicnaKarta.KorisnikAplikacije);
            ViewBag.KorisnikLicneKarte = new SelectList(db.tblKorisnikLicneKartes, "Id", "Ime", tblLicnaKarta.KorisnikLicneKarte);
            return View(tblLicnaKarta);
        }

        // GET: tblLicnaKarta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLicnaKarta tblLicnaKarta = db.tblLicnaKartas.Find(id);
            if (tblLicnaKarta == null)
            {
                return HttpNotFound();
            }
            return View(tblLicnaKarta);
        }

        // POST: tblLicnaKarta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblLicnaKarta tblLicnaKarta = db.tblLicnaKartas.Find(id);
            db.tblLicnaKartas.Remove(tblLicnaKarta);
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
