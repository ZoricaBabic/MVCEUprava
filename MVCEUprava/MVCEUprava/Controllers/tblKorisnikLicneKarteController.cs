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
    public class tblKorisnikLicneKarteController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblKorisnikLicneKarte
        public ActionResult Index()
        {
            var tblKorisnikLicneKartes = db.tblKorisnikLicneKartes.Include(t => t.tblOtisak).Include(t => t.tblPotpi).Include(t => t.tblSlika);
            return View(tblKorisnikLicneKartes.ToList());
        }

        // GET: tblKorisnikLicneKarte/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKorisnikLicneKarte tblKorisnikLicneKarte = db.tblKorisnikLicneKartes.Find(id);
            if (tblKorisnikLicneKarte == null)
            {
                return HttpNotFound();
            }
            return View(tblKorisnikLicneKarte);
        }

        // GET: tblKorisnikLicneKarte/Create
        public ActionResult Create()
        {
            ViewBag.Otisak = new SelectList(db.tblOtisaks, "Id", "Id");
            ViewBag.Potpis = new SelectList(db.tblPotpis, "Id", "Id");
            ViewBag.Slika = new SelectList(db.tblSlikas, "Id", "Id");
            return View();
        }

        // POST: tblKorisnikLicneKarte/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ime,Prezime,DatumRodjenja,Jmbg,Pol,Adresa,Mesto,Potpis,Otisak,Slika")] tblKorisnikLicneKarte tblKorisnikLicneKarte)
        {
            if (ModelState.IsValid)
            {
                db.tblKorisnikLicneKartes.Add(tblKorisnikLicneKarte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Otisak = new SelectList(db.tblOtisaks, "Id", "Id", tblKorisnikLicneKarte.Otisak);
            ViewBag.Potpis = new SelectList(db.tblPotpis, "Id", "Id", tblKorisnikLicneKarte.Potpis);
            ViewBag.Slika = new SelectList(db.tblSlikas, "Id", "Id", tblKorisnikLicneKarte.Slika);
            return View(tblKorisnikLicneKarte);
        }

        // GET: tblKorisnikLicneKarte/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKorisnikLicneKarte tblKorisnikLicneKarte = db.tblKorisnikLicneKartes.Find(id);
            if (tblKorisnikLicneKarte == null)
            {
                return HttpNotFound();
            }
            ViewBag.Otisak = new SelectList(db.tblOtisaks, "Id", "Id", tblKorisnikLicneKarte.Otisak);
            ViewBag.Potpis = new SelectList(db.tblPotpis, "Id", "Id", tblKorisnikLicneKarte.Potpis);
            ViewBag.Slika = new SelectList(db.tblSlikas, "Id", "Id", tblKorisnikLicneKarte.Slika);
            return View(tblKorisnikLicneKarte);
        }

        // POST: tblKorisnikLicneKarte/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ime,Prezime,DatumRodjenja,Jmbg,Pol,Adresa,Mesto,Potpis,Otisak,Slika")] tblKorisnikLicneKarte tblKorisnikLicneKarte)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKorisnikLicneKarte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Otisak = new SelectList(db.tblOtisaks, "Id", "Id", tblKorisnikLicneKarte.Otisak);
            ViewBag.Potpis = new SelectList(db.tblPotpis, "Id", "Id", tblKorisnikLicneKarte.Potpis);
            ViewBag.Slika = new SelectList(db.tblSlikas, "Id", "Id", tblKorisnikLicneKarte.Slika);
            return View(tblKorisnikLicneKarte);
        }

        // GET: tblKorisnikLicneKarte/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKorisnikLicneKarte tblKorisnikLicneKarte = db.tblKorisnikLicneKartes.Find(id);
            if (tblKorisnikLicneKarte == null)
            {
                return HttpNotFound();
            }
            return View(tblKorisnikLicneKarte);
        }

        // POST: tblKorisnikLicneKarte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblKorisnikLicneKarte tblKorisnikLicneKarte = db.tblKorisnikLicneKartes.Find(id);
            db.tblKorisnikLicneKartes.Remove(tblKorisnikLicneKarte);
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
