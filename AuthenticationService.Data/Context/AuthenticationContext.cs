using AuthenticationService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Data.Context
{
    public class AuthenticationContext : DbContext
    {
        #region constants
        private const string _USUARIO_TABLE_NAME = "Usuarios";
        #endregion

        #region configurations
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=tcp:pradoserver.database.windows.net,1433;Initial Catalog=pradoDataBase;Persist Security Info=False;User ID=bruno.prado;Password=9933@pipo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Telefones)
                .WithOne(t => t.User);
            modelBuilder.Entity<Usuario>().ToTable<Usuario>(_USUARIO_TABLE_NAME);            
        }
        #endregion

        #region DBSets
        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<Telefone> Telefones { get; set; }
        #endregion
    }
}
