using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {   
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<PlanoModel> Planos { get; set; }
    public DbSet<BeneficiarioModel> Beneficiarios { get; set; }

    }
}
