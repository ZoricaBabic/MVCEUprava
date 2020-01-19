//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCEUprava.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class tblLicnaKarta
    {
        public int Id { get; set; }
        [DisplayName("Registarski broj")]
        public string RegistarskiBroj { get; set; }
        [DisplayName("Datum izdavanja")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime DatumIzdavanja { get; set; }
        [DisplayName("Datum isteka")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime DatumIsteka { get; set; }
        public int KorisnikLicneKarte { get; set; }
        public int KorisnikAplikacije { get; set; }
        public virtual tblKorisnikAplikacije tblKorisnikAplikacije { get; set; }
        public virtual tblKorisnikLicneKarte tblKorisnikLicneKarte { get; set; }
    }
}
