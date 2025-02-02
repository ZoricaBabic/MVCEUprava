﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVCEUprava.App_Start;
using MVCEUprava.Models;

namespace MVCEUprava.Controllers
{
    public class tblKorisnikLicneKarteController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblKorisnikLicneKarte
        [AllUsersAuthorize]
        public ActionResult Index()
        {
            //var tblKorisnikLicneKartes = db.tblKorisnikLicneKartes.Include(t => t.tblOtisak).Include(t => t.tblPotpi).Include(t => t.tblSlika);
            //return View(tblKorisnikLicneKartes.ToList());
            return RedirectToAction("Index", "tblLicnaKarta");
        }
        // GET: tblKorisnikLicneKarte/Create
        [KorisnikAplikacijeAuthorize]
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
        [KorisnikAplikacijeAuthorize]
        public ActionResult Create([Bind(Include = "Id,Ime,Prezime,DatumRodjenja,Jmbg,Pol,Adresa,Mesto,Potpis,Otisak,Slika,SlikaKorisnika,OtisakKorisnika,PotpisKorisnika")] tblKorisnikLicneKarte tblKorisnikLicneKarte)
        {
            if(tblKorisnikLicneKarte.SlikaKorisnika == null)
            {
                ModelState.AddModelError("SlikaKorisnika", "Slika mora da bude 189x189 i formata .jpg, .png, .gif ili .jpeg");
                return View(tblKorisnikLicneKarte);
            }

            if (tblKorisnikLicneKarte.OtisakKorisnika == null)
            {
                ModelState.AddModelError("OtisakKorisnika", "Otisak mora da bude 189x189 i formata .jpg, .png, .gif ili .jpeg");
                return View(tblKorisnikLicneKarte);
            }

            if (tblKorisnikLicneKarte.PotpisKorisnika == null)
            {
                ModelState.AddModelError("PotpisKorisnika", "Potpis mora da bude 189x189 i formata .jpg, .png, .gif ili .jpeg");
                return View(tblKorisnikLicneKarte);
            }
            tblPotpi potpis;
            tblOtisak otisak;
            tblSlika slika;
            if (ModelState.IsValid)
            {
                //validate jmbg
                if(db.tblKorisnikLicneKartes.Where(x=> x.Jmbg == tblKorisnikLicneKarte.Jmbg).Count() > 0)
                {
                    ModelState.AddModelError("Jmbg", "Korisnik sa unetim JMBG-om već postoji.");
                    return View(tblKorisnikLicneKarte);
                }
                //slika
                if (ValidateImage(tblKorisnikLicneKarte.SlikaKorisnika))
                {
                    //image
                    byte[] imageByte = null;
                    using (var binaryReader = new BinaryReader(Request.Files["SlikaKorisnika"].InputStream))
                    {
                        //image = binaryReader.ReadBytes(Request.Files["SlikaKorisnika"].ContentLength);
                        Image image = Image.FromStream(tblKorisnikLicneKarte.SlikaKorisnika.InputStream, true, true);
                        imageByte = ImageToByte(image);
                    }

                    slika = new tblSlika();
                    slika.Id = 0;
                    slika.Slika = imageByte;
                    db.tblSlikas.Add(slika);
                }
                else
                {
                    ModelState.AddModelError("SlikaKorisnika", "Slika mora da bude 189x189 i formata .jpg, .png, .gif ili .jpeg");
                    return View(tblKorisnikLicneKarte);
                }
                //243x104 potpis
                if (ValidateImage(tblKorisnikLicneKarte.PotpisKorisnika))
                {
                    //image
                    byte[] image = null;
                    using (var binaryReader = new BinaryReader(Request.Files["PotpisKorisnika"].InputStream))
                    {
                        //image = binaryReader.ReadBytes(Request.Files["SlikaKorisnika"].ContentLength);
                        Image i = Image.FromStream(tblKorisnikLicneKarte.PotpisKorisnika.InputStream, true, true);
                        image = ImageToByte(i);
                    }

                    potpis = new tblPotpi();
                    potpis.Id = 0;
                    potpis.Potpis = image;
                    db.tblPotpis.Add(potpis);
                }
                else
                {
                    ModelState.AddModelError("PotpisKorisnika", "Potpis korisnika mora da bude 189x189  i formata .jpg, .png, .gif ili .jpeg");
                    return View(tblKorisnikLicneKarte);
                }

                //612x509 otisak
                if (ValidateImage(tblKorisnikLicneKarte.OtisakKorisnika))
                {
                    //image
                    byte[] image = null;
                    using (var binaryReader = new BinaryReader(Request.Files["OtisakKorisnika"].InputStream))
                    {
                        Image i = Image.FromStream(tblKorisnikLicneKarte.OtisakKorisnika.InputStream, true, true);
                        image = ImageToByte(i);
                    }

                    otisak = new tblOtisak();
                    otisak.Id = 0;
                    otisak.Otisak = image;
                    db.tblOtisaks.Add(otisak);
                }
                else
                {
                    ModelState.AddModelError("OtisakKorisnika", "Otisak korisnika mora da bude 189x189 i formata .jpg, .png, .gif ili .jpeg");
                    return View(tblKorisnikLicneKarte);
                }

                DateTime? datumisteka = CalculateDatumIsteka(tblKorisnikLicneKarte.DatumRodjenja);
                if (datumisteka == null)
                {
                    ModelState.AddModelError("DatumRodjenja", "Licna karta ne moze da se napravi za osobu koja je mladja od 10 godina.");
                    return View(tblKorisnikLicneKarte);
                }

                db.SaveChanges();
                tblKorisnikLicneKarte.Potpis = potpis.Id;
                tblKorisnikLicneKarte.Otisak = otisak.Id;
                tblKorisnikLicneKarte.Slika = slika.Id;
                db.tblKorisnikLicneKartes.Add(tblKorisnikLicneKarte);
                db.SaveChanges();
                tblLicnaKarta tblLicnaKarta = new tblLicnaKarta();
                tblLicnaKarta.RegistarskiBroj = GenerateRegistarskiBroj();
                while(db.tblLicnaKartas.Where(x=> x.RegistarskiBroj == tblLicnaKarta.RegistarskiBroj).ToList().Count() > 0)
                {
                    tblLicnaKarta.RegistarskiBroj = GenerateRegistarskiBroj();
                }
                tblLicnaKarta.DatumIzdavanja = DateTime.Now.Date;
                tblLicnaKarta.DatumIsteka = datumisteka.Value;
                tblLicnaKarta.KorisnikLicneKarte = tblKorisnikLicneKarte.Id;
                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;
                tblLicnaKarta.KorisnikAplikacije = db.tblKorisnikAplikacijes.Where(x => x.Email == name).Select(x=> x.Id).FirstOrDefault();
                db.tblLicnaKartas.Add(tblLicnaKarta);
                db.SaveChanges();
                return RedirectToAction("Index","tblLicnaKarta");
            }

            ViewBag.Otisak = new SelectList(db.tblOtisaks, "Id", "Id", tblKorisnikLicneKarte.Otisak);
            ViewBag.Potpis = new SelectList(db.tblPotpis, "Id", "Id", tblKorisnikLicneKarte.Potpis);
            ViewBag.Slika = new SelectList(db.tblSlikas, "Id", "Id", tblKorisnikLicneKarte.Slika);
            return View(tblKorisnikLicneKarte);
        }
        // GET: tblKorisnikLicneKarte/Edit/5
        [KorisnikAplikacijeAuthorize]
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
        [KorisnikAplikacijeAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ime,Prezime,DatumRodjenja,Jmbg,Pol,Adresa,Mesto,Potpis,Otisak,Slika,SlikaKorisnika,OtisakKorisnika,PotpisKorisnika")] tblKorisnikLicneKarte tblKorisnikLicneKarte)
        {
            tblPotpi potpis = db.tblKorisnikLicneKartes.Where(x => x.Id == tblKorisnikLicneKarte.Id).Select(x => x.tblPotpi).FirstOrDefault();
            tblOtisak otisak = db.tblKorisnikLicneKartes.Where(x => x.Id == tblKorisnikLicneKarte.Id).Select(x => x.tblOtisak).FirstOrDefault();
            tblSlika slika = db.tblKorisnikLicneKartes.Where(x => x.Id == tblKorisnikLicneKarte.Id).Select(x => x.tblSlika).FirstOrDefault();
            tblKorisnikLicneKarte.Potpis = potpis.Id;
            tblKorisnikLicneKarte.Otisak = otisak.Id;
            tblKorisnikLicneKarte.Slika = slika.Id;
            if (ModelState.IsValid)
            {
                //validate jmbg
                if (db.tblKorisnikLicneKartes.Where(x => x.Jmbg == tblKorisnikLicneKarte.Jmbg && x.Id != tblKorisnikLicneKarte.Id).Count() > 0)
                {
                    ModelState.AddModelError("Jmbg", "Korisnik sa unetim JMBG-om već postoji.");
                    return View(tblKorisnikLicneKarte);
                }
                //slika
                if (tblKorisnikLicneKarte.SlikaKorisnika!= null && ValidateImage(tblKorisnikLicneKarte.SlikaKorisnika))
                {
                    //image
                    byte[] imageByte = null;
                    using (var binaryReader = new BinaryReader(Request.Files["SlikaKorisnika"].InputStream))
                    {
                        //image = binaryReader.ReadBytes(Request.Files["SlikaKorisnika"].ContentLength);
                        Image image = Image.FromStream(tblKorisnikLicneKarte.SlikaKorisnika.InputStream, true, true);
                        imageByte = ImageToByte(image);
                    }
                    slika.Slika = imageByte;
                    db.Entry(slika).State = EntityState.Modified;
                }
                else if (tblKorisnikLicneKarte.SlikaKorisnika != null)
                {
                    ModelState.AddModelError("SlikaKorisnika", "Slika mora da bude 189x189 i formata .jpg, .png, .gif ili .jpeg");
                    return View(tblKorisnikLicneKarte);
                }
                //243x104 potpis
                if (tblKorisnikLicneKarte.PotpisKorisnika != null && ValidateImage(tblKorisnikLicneKarte.PotpisKorisnika))
                {
                    //image
                    byte[] image = null;
                    using (var binaryReader = new BinaryReader(Request.Files["PotpisKorisnika"].InputStream))
                    {
                        //image = binaryReader.ReadBytes(Request.Files["SlikaKorisnika"].ContentLength);
                        Image i = Image.FromStream(tblKorisnikLicneKarte.PotpisKorisnika.InputStream, true, true);
                        image = ImageToByte(i);
                    }

                    potpis.Potpis = image;
                    db.Entry(potpis).State = EntityState.Modified;
                }
                else if(tblKorisnikLicneKarte.PotpisKorisnika != null)
                {
                    ModelState.AddModelError("PotpisKorisnika", "Potpis korisnika mora da bude 189x189  i formata .jpg, .png, .gif ili .jpeg");
                    return View(tblKorisnikLicneKarte);
                }

                //612x509 otisak
                if (tblKorisnikLicneKarte.OtisakKorisnika != null && ValidateImage(tblKorisnikLicneKarte.OtisakKorisnika))
                {
                    //image
                    byte[] image = null;
                    using (var binaryReader = new BinaryReader(Request.Files["OtisakKorisnika"].InputStream))
                    {
                        Image i = Image.FromStream(tblKorisnikLicneKarte.OtisakKorisnika.InputStream, true, true);
                        image = ImageToByte(i);
                    }
                    otisak.Otisak = image;
                    db.Entry(otisak).State = EntityState.Modified;
                }
                else if(tblKorisnikLicneKarte.OtisakKorisnika != null)
                {
                    ModelState.AddModelError("OtisakKorisnika", "Otisak korisnika mora da bude 189x189 i formata .jpg, .png, .gif ili .jpeg");
                    return View(tblKorisnikLicneKarte);
                }

                db.SaveChanges();
                tblKorisnikLicneKarte.Slika = slika.Id;
                tblKorisnikLicneKarte.Potpis = potpis.Id;
                tblKorisnikLicneKarte.Otisak = slika.Id;
                db.Entry(tblKorisnikLicneKarte).State = EntityState.Modified;
                db.SaveChanges();
                tblLicnaKarta tblLicnaKarta = db.tblLicnaKartas.Where(x => x.KorisnikLicneKarte == tblKorisnikLicneKarte.Id).FirstOrDefault();
                tblLicnaKarta.KorisnikLicneKarte = tblKorisnikLicneKarte.Id;
                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;
                tblLicnaKarta.KorisnikAplikacije = db.tblKorisnikAplikacijes.Where(x => x.Email == name).Select(x => x.Id).FirstOrDefault();
                db.Entry(tblLicnaKarta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "tblLicnaKarta");
            }
            ViewBag.Otisak = new SelectList(db.tblOtisaks, "Id", "Id", tblKorisnikLicneKarte.Otisak);
            ViewBag.Potpis = new SelectList(db.tblPotpis, "Id", "Id", tblKorisnikLicneKarte.Potpis);
            ViewBag.Slika = new SelectList(db.tblSlikas, "Id", "Id", tblKorisnikLicneKarte.Slika);
            return View(tblKorisnikLicneKarte);
        }

        // GET: tblKorisnikLicneKarte/Delete/5
        [KorisnikAplikacijeAuthorize]
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
        [KorisnikAplikacijeAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
          
            tblKorisnikLicneKarte tblKorisnikLicneKarte = db.tblKorisnikLicneKartes.Find(id);
            tblLicnaKarta tblLicna = db.tblLicnaKartas.Where(x => x.tblKorisnikLicneKarte.Id == tblKorisnikLicneKarte.Id).FirstOrDefault();
            db.tblLicnaKartas.Remove(tblLicna);
            db.tblKorisnikLicneKartes.Remove(tblKorisnikLicneKarte);
            db.SaveChanges();
            return RedirectToAction("Index", "tblLicnaKarta");
        }

        private bool ValidateImage(HttpPostedFileBase image)
        {
            if (IsImage(image))
            {
                Image i = Image.FromStream(image.InputStream, true, true);
                if (i.Height == 189 && i.Width == 189)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private string GenerateRegistarskiBroj()
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < 9; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        private DateTime? CalculateDatumIsteka(DateTime datumRodjenja)
        {
            
            var today = DateTime.Today;
            var age = today.Year - datumRodjenja.Year;
            if (datumRodjenja.Date > today.AddYears(-age))
            {
                age--;
            }

            if (age < 10)
            {
                return null;
            }

            if (age >= 18)
            {
                return DateTime.Now.Date.AddYears(10);
            } else
            {
                return DateTime.Now.Date.AddYears(5);
            }
        }

        //public FileContentResult Show(int id, string type)
        //{
        //    LicneKarteDBEntities db = new LicneKarteDBEntities();
        //    byte[] imageData = null;
        //    if (type == "slika")
        //    {
        //        imageData = db.tblSlikas.Where(x => x.Id == id).Select(x => x.Slika).FirstOrDefault();
        //    }

        //    if (type == "otisak")
        //    {
        //        imageData = db.tblOtisaks.Where(x => x.Id == id).Select(x => x.Otisak).FirstOrDefault();
        //    }

        //    if (type == "potpis")
        //    {
        //        imageData = db.tblPotpis.Where(x => x.Id == id).Select(x => x.Potpis).FirstOrDefault();
        //    }
        //    return File(imageData, "image/jpg");
        //}

        private byte[] ImageToByte(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
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
