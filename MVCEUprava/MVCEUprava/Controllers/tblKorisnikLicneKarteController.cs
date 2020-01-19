using System;
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
using MVCEUprava.Models;

namespace MVCEUprava.Controllers
{
    public class tblKorisnikLicneKarteController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblKorisnikLicneKarte
        public ActionResult Index()
        {
            //var tblKorisnikLicneKartes = db.tblKorisnikLicneKartes.Include(t => t.tblOtisak).Include(t => t.tblPotpi).Include(t => t.tblSlika);
            //return View(tblKorisnikLicneKartes.ToList());
            return RedirectToAction("Index", "tblLicnaKarta");
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
        public ActionResult Create([Bind(Include = "Id,Ime,Prezime,DatumRodjenja,Jmbg,Pol,Adresa,Mesto,Potpis,Otisak,Slika,SlikaKorisnika,OtisakKorisnika,PotpisKorisnika")] tblKorisnikLicneKarte tblKorisnikLicneKarte)
        {
            tblPotpi potpis;
            tblOtisak otisak;
            tblSlika slika;
            if (ModelState.IsValid)
            {
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
