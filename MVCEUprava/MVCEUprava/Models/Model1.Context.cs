﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LicneKarteDBEntities : DbContext
    {
        public LicneKarteDBEntities()
            : base("name=LicneKarteDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblIzdavalac> tblIzdavalacs { get; set; }
        public virtual DbSet<tblKorisnikAplikacije> tblKorisnikAplikacijes { get; set; }
        public virtual DbSet<tblKorisnikLicneKarte> tblKorisnikLicneKartes { get; set; }
        public virtual DbSet<tblLicnaKarta> tblLicnaKartas { get; set; }
        public virtual DbSet<tblOtisak> tblOtisaks { get; set; }
        public virtual DbSet<tblPotpi> tblPotpis { get; set; }
        public virtual DbSet<tblSlika> tblSlikas { get; set; }
        public virtual DbSet<vwIzdavalac> vwIzdavalacs { get; set; }
        public virtual DbSet<vwKorisnikAplikacije> vwKorisnikAplikacijes { get; set; }
        public virtual DbSet<vwKorisnikLicneKarte> vwKorisnikLicneKartes { get; set; }
        public virtual DbSet<vwLicnaKarta> vwLicnaKartas { get; set; }
    }
}
