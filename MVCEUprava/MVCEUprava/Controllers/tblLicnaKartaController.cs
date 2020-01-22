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
        [AllUsersAuthorize]
        public ActionResult Index()
        {
            List<tblLicnaKarta> tblLicnaKartas = new List<tblLicnaKarta>();
            tblLicnaKartas = db.tblLicnaKartas.Include(t => t.tblKorisnikAplikacije).Include(t => t.tblKorisnikLicneKarte).ToList();
            if(tblLicnaKartas.Count() == 0)
            {
                tblLicnaKartas = db.tblLicnaKartas.Include(t => t.tblKorisnikLicneKarte).ToList();
            }
            return View(tblLicnaKartas.ToList());
        }

        // GET: tblLicnaKarta/Create
        [KorisnikAplikacijeAuthorize]
        public ActionResult Create()
        {
            return RedirectToAction("Create", "tblKorisnikLicnekarte");
        }

        // GET: tblLicnaKarta/Edit/5
        [KorisnikAplikacijeAuthorize]
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
            return RedirectToAction("Edit", "tblKorisnikLicneKarte", new { id = tblLicnaKarta.tblKorisnikLicneKarte.Id });
        }

        // GET: tblLicnaKarta/Delete/5
        [KorisnikAplikacijeAuthorize]
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

            return RedirectToAction("Delete", "tblKorisnikLicneKarte", new { id = tblLicnaKarta.tblKorisnikLicneKarte.Id });
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
