using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVCEUprava.App_Start;
using MVCEUprava.Models;

namespace MVCEUprava.Controllers
{
    public class tblKorisnikAplikacijeController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        //GET: tblKorisnikAplikacije
        [KorisnikAplikacijeAuthorize]
        public ActionResult Index()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;
            var tblKorisnikAplikacijes = db.tblKorisnikAplikacijes.Include(t => t.tblIzdavalac).Where(x=> x.Email == name).ToList();
            return View(tblKorisnikAplikacijes.ToList());
        }

        //Get registration
        [HttpGet]
        public ActionResult Registration()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
            {
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv");
                return View();
            }
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;

            if (db.tblKorisnikAplikacijes.Where(x => x.Email == name).ToList().Count() == 0)
            {
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv");
                return View();
            }
            return RedirectToAction("Index", "tblLicnaKarta");
        }

        //Get login
        [HttpGet]
        public ActionResult Login()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
            {
                return View();
            }
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;

            if (db.tblKorisnikAplikacijes.Where(x => x.Email == name).ToList().Count() == 0)
            {
                return View();
            }
            return RedirectToAction("Index", "tblLicnaKarta");
        }

        //Get login
        [HttpGet]
        [AllUsersAuthorize]
        public ActionResult Logout()
        {
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Registration", "tblKorisnikAplikacije");
        }

        //Login
        [HttpPost]
        public ActionResult LoginUser([Bind(Include = "Email,Password")] tblKorisnikAplikacije tblKorisnikAplikacije)
        {
            if(tblKorisnikAplikacije.Email == "admin@gmail.com" && tblKorisnikAplikacije.Password == "admin")
            {
                FormsAuthentication.SetAuthCookie(tblKorisnikAplikacije.Email, false);
                return RedirectToAction("Index", "tblLicnaKarta");
            }
            if(db.tblKorisnikAplikacijes.Where(x=> x.Email == tblKorisnikAplikacije.Email).ToList().Count() == 0)
            {
                ModelState.AddModelError("Email", "Napostojeća e-mail adresa.");
                return View("Login",tblKorisnikAplikacije);
            }

            if(db.tblKorisnikAplikacijes.Where(x=>x.Email == tblKorisnikAplikacije.Email && x.Password==tblKorisnikAplikacije.Password).ToList().Count() == 1)
            {
                FormsAuthentication.SetAuthCookie(tblKorisnikAplikacije.Email,false);
                return RedirectToAction("Index","tblLicnaKarta");
            }

            return View("Login", tblKorisnikAplikacije);
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
                FormsAuthentication.SetAuthCookie(tblKorisnikAplikacije.Email, false);
                return RedirectToAction("Index", "tblLicnaKarta");

            }

            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Id", tblKorisnikAplikacije.Poslodavac);
            return View(tblKorisnikAplikacije);
        }

        // GET: tblKorisnikAplikacije/Edit/5
        [KorisnikAplikacijeAuthorize]
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
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;
            if (db.tblKorisnikAplikacijes.Where(x => x.Email == name && x.Id == tblKorisnikAplikacije.Id).ToList().Count() != 1)
            {
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv");
                return View(tblKorisnikAplikacije);
            }

            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv");
            return View(tblKorisnikAplikacije);
        }

        // POST: tblKorisnikAplikacije/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [KorisnikAplikacijeAuthorize]
        public ActionResult Edit([Bind(Include = "Id,Ime,Prezime,Pol,Jmbg,Adresa,Mesto,BrojTelefona,DatumRodjenja,Email,Password,Poslodavac,ConfirmPassword")] tblKorisnikAplikacije tblKorisnikAplikacije)
        {

            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;

            if (tblKorisnikAplikacije.Password != tblKorisnikAplikacije.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Pogrešan unos.");
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv", tblKorisnikAplikacije.Poslodavac);
                return View(tblKorisnikAplikacije);
            }

            if (db.tblKorisnikAplikacijes.Where(x => x.Email == tblKorisnikAplikacije.Email).ToList().Count() > 1)
            {
                ModelState.AddModelError("Email", "Korisnik sa unsenim e-mail-om već postoji.");
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv", tblKorisnikAplikacije.Poslodavac);
                return View(tblKorisnikAplikacije);
            }

            if (db.tblKorisnikAplikacijes.Where(x => x.Jmbg == tblKorisnikAplikacije.Jmbg && x.Email != name).ToList().Count() > 0)
            {
                ModelState.AddModelError("Jmbg", "Korisnik sa unsenim JMBG-om već postoji.");
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv", tblKorisnikAplikacije.Poslodavac);
                return View(tblKorisnikAplikacije);
            }
            if (ModelState.IsValid)
            {
                if (db.tblKorisnikAplikacijes.Where(x => x.Email == name && x.Id == tblKorisnikAplikacije.Id).ToList().Count() != 1)
                {
                    ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Id", tblKorisnikAplikacije.Poslodavac);
                    return View(tblKorisnikAplikacije);
                }
                tblKorisnikAplikacije tblKorisnikAplikacijeUpdated = db.tblKorisnikAplikacijes.Where(x => x.Id == tblKorisnikAplikacije.Id).FirstOrDefault();
                tblKorisnikAplikacijeUpdated.Id = tblKorisnikAplikacije.Id;
                tblKorisnikAplikacijeUpdated.Ime = tblKorisnikAplikacije.Ime;
                tblKorisnikAplikacijeUpdated.Prezime = tblKorisnikAplikacije.Prezime;
                tblKorisnikAplikacijeUpdated.Pol = tblKorisnikAplikacije.Pol;
                tblKorisnikAplikacijeUpdated.Jmbg = tblKorisnikAplikacije.Jmbg;
                tblKorisnikAplikacijeUpdated.Adresa = tblKorisnikAplikacije.Adresa;
                tblKorisnikAplikacijeUpdated.Mesto = tblKorisnikAplikacije.Mesto;
                tblKorisnikAplikacijeUpdated.BrojTelefona = tblKorisnikAplikacije.BrojTelefona;
                tblKorisnikAplikacijeUpdated.DatumRodjenja = tblKorisnikAplikacije.DatumRodjenja;
                tblKorisnikAplikacijeUpdated.Email = tblKorisnikAplikacije.Email;
                tblKorisnikAplikacijeUpdated.Password = tblKorisnikAplikacije.Password;
                tblKorisnikAplikacijeUpdated.Poslodavac = tblKorisnikAplikacije.Poslodavac;
                db.Entry(tblKorisnikAplikacijeUpdated).State = EntityState.Modified;
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(tblKorisnikAplikacije.Email, false);
                return RedirectToAction("Index");
            }

            ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv");
            return View(tblKorisnikAplikacije);
        }

        // GET: tblKorisnikAplikacije/Delete/5
        [KorisnikAplikacijeAuthorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblKorisnikAplikacije tblKorisnikAplikacije = db.tblKorisnikAplikacijes.Find(id);
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;
            if (db.tblKorisnikAplikacijes.Where(x => x.Email == name && x.Id == tblKorisnikAplikacije.Id).ToList().Count() != 1)
            {
                ViewBag.Poslodavac = new SelectList(db.tblIzdavalacs, "Id", "Naziv");
                return View(tblKorisnikAplikacije);
            }
            if (tblKorisnikAplikacije == null)
            {
                return HttpNotFound();
            }
            return View(tblKorisnikAplikacije);
        }

        // POST: tblKorisnikAplikacije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [KorisnikAplikacijeAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;
            tblKorisnikAplikacije tblKorisnikAplikacije = db.tblKorisnikAplikacijes.Find(id);
            if (db.tblKorisnikAplikacijes.Where(x => x.Email == name && x.Id == id).ToList().Count() != 1)
            {
                ModelState.AddModelError("Message", "Ne možete izvršiti modifikaciju za drugog korisnika.");
                return View(tblKorisnikAplikacije);
                
            }
            db.tblKorisnikAplikacijes.Remove(tblKorisnikAplikacije);

            db.SaveChanges();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Registration", "tblKorisnikAplikacije");
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
