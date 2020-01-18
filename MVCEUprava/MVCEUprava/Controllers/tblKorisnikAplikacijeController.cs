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
    public class tblKorisnikAplikacijeController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblKorisnikAplikacije
        public ActionResult Index()
        {
            var tblKorisnikAplikacijes = db.tblKorisnikAplikacijes.Include(t => t.tblIzdavalac);
            return View(tblKorisnikAplikacijes.ToList());
        }

        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv");
            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // GET: tblKorisnikAplikacije/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKorisnikAplikacije tblKorisnikAplikacije = db.tblKorisnikAplikacijes.Find(id);
            if (tblKorisnikAplikacije == null)
            {
                return HttpNotFound();
            }
            return View(tblKorisnikAplikacije);
        }

        // GET: tblKorisnikAplikacije/Create
        public ActionResult Create()
        {
            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Id");
            return View();
        }

        //Register
        // POST: tblKorisnikAplikacije/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "Id,Ime,Prezime,Pol,Jmbg,Adresa,Mesto,BrojTelefona,DatumRodjenja,Email,Password,Poslodavac,ConfirmPassword")] tblKorisnikAplikacije tblKorisnikAplikacije)
        {
            if(tblKorisnikAplikacije.Password != tblKorisnikAplikacije.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Pogrešan unos.");
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv", tblKorisnikAplikacije.Poslodavac);
                return View(tblKorisnikAplikacije);
            }

            if(db.tblKorisnikAplikacijes.Where(x=>x.Email == tblKorisnikAplikacije.Email).ToList().Count() > 0)
            {
                ModelState.AddModelError("Email", "Korisnik sa unsenim e-mail-om već postoji.");
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv", tblKorisnikAplikacije.Poslodavac);
                return View(tblKorisnikAplikacije);
            }

            if (db.tblKorisnikAplikacijes.Where(x => x.Jmbg == tblKorisnikAplikacije.Jmbg).ToList().Count() > 0)
            {
                ModelState.AddModelError("Jmbg", "Korisnik sa unsenim JMBG-om već postoji.");
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv", tblKorisnikAplikacije.Poslodavac);
                return View(tblKorisnikAplikacije);
            }

            if (ModelState.IsValid)
            {
                db.tblKorisnikAplikacijes.Add(tblKorisnikAplikacije);
                db.SaveChanges();
                return RedirectToAction("Index","tblLicnaKarta");

            }
            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Id", tblKorisnikAplikacije.Poslodavac);
            return View(tblKorisnikAplikacije);
        }

        // GET: tblKorisnikAplikacije/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKorisnikAplikacije tblKorisnikAplikacije = db.tblKorisnikAplikacijes.Find(id);
            if (tblKorisnikAplikacije == null)
            {
                return HttpNotFound();
            }
            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Id", tblKorisnikAplikacije.Poslodavac);
            return View(tblKorisnikAplikacije);
        }

        // POST: tblKorisnikAplikacije/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ime,Prezime,Pol,Jmbg,Adresa,Mesto,BrojTelefona,DatumRodjenja,Email,Password,Poslodavac")] tblKorisnikAplikacije tblKorisnikAplikacije)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKorisnikAplikacije).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Id", tblKorisnikAplikacije.Poslodavac);
            return View(tblKorisnikAplikacije);
        }

        // GET: tblKorisnikAplikacije/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKorisnikAplikacije tblKorisnikAplikacije = db.tblKorisnikAplikacijes.Find(id);
            if (tblKorisnikAplikacije == null)
            {
                return HttpNotFound();
            }
            return View(tblKorisnikAplikacije);
        }

        // POST: tblKorisnikAplikacije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblKorisnikAplikacije tblKorisnikAplikacije = db.tblKorisnikAplikacijes.Find(id);
            db.tblKorisnikAplikacijes.Remove(tblKorisnikAplikacije);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Registration


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
