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
    public class tblIzdavalacController : Controller
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();

        // GET: tblIzdavalac
        [AdminAuthorize]
        public ActionResult Index()
        {
            return View(db.tblIzdavalacs.ToList());
        }

        // GET: tblIzdavalac/Details/5
        //[AdminAuthorize]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblIzdavalac tblIzdavalac = db.tblIzdavalacs.Find(id);
        //    if (tblIzdavalac == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblIzdavalac);
        //}

        // GET: tblIzdavalac/Create
        [AdminAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: tblIzdavalac/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public ActionResult Create([Bind(Include = "Id,Naziv")] tblIzdavalac tblIzdavalac)
        {
            if (ModelState.IsValid)
            {
                if(db.tblIzdavalacs.Where(x=>x.Naziv == tblIzdavalac.Naziv).ToList().Count() > 0)
                {
                    ModelState.AddModelError("Naziv", "Izdavalac sa zadatim nazivom već postoji.");
                    return View(tblIzdavalac);
                }
                db.tblIzdavalacs.Add(tblIzdavalac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblIzdavalac);
        }

        // GET: tblIzdavalac/Edit/5
        [AdminAuthorize]
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
        [AdminAuthorize]
        public ActionResult Edit([Bind(Include = "Id,Naziv")] tblIzdavalac tblIzdavalac)
        {
            if (ModelState.IsValid)
            {
                if (db.tblIzdavalacs.Where(x => x.Naziv == tblIzdavalac.Naziv && x.Id !=tblIzdavalac.Id).ToList().Count() > 0)
                {
                    ModelState.AddModelError("Naziv", "Izdavalac sa zadatim nazivom već postoji.");
                    return View(tblIzdavalac);
                }
                db.Entry(tblIzdavalac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblIzdavalac);
        }

        // GET: tblIzdavalac/Delete/5
        [AdminAuthorize]
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
        [AdminAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            tblIzdavalac tblIzdavalac = db.tblIzdavalacs.Find(id);
            if (db.tblKorisnikAplikacijes.Where(x => x.tblIzdavalac.Id == id).ToList().Count() > 0)
            {
                ModelState.AddModelError("Naziv", "Izdavalac u upotrebi.");
                return View(tblIzdavalac);
            }
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
