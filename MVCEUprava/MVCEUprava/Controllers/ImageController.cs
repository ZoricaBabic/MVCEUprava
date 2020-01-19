using MVCEUprava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCEUprava.Controllers
{
    public class ImageController : Controller
    {

        public ActionResult Show(int id,string type)
        {
            LicneKarteDBEntities db = new LicneKarteDBEntities();
            byte[] imageData = null;
            if (type == "slika")
            {
                imageData = db.tblSlikas.Where(x => x.Id == id).Select(x=>x.Slika).FirstOrDefault();
                
            }

            if (type == "otisak")
            {
                imageData = db.tblOtisaks.Where(x => x.Id == id).Select(x => x.Otisak).FirstOrDefault();
            }

            if (type == "potpis")
            {
                imageData = db.tblPotpis.Where(x => x.Id == id).Select(x => x.Potpis).FirstOrDefault();
            }
            return File(imageData, "image/jpg");
        }
        
    }
}