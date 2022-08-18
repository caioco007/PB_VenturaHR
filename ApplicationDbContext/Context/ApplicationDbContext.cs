using ApplicationDbContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDbContext.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vaga> Vaga { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Candidato> Candidato { get; set; }
    }
}
