using API_CadastroSimples.Models;
using API_CadastroSimples.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace API_CadastroSimples.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.Property(p => p.Nome)
                    .HasMaxLength(100); // Define o tamanho máximo do campo Nome como 100 caracteres

                entity.Property(p => p.Sexo)
                    .IsRequired(false) // Define que a coluna pode ser nula
                    .HasColumnType("varchar(1)") // Define o tipo da coluna como varchar com comprimento de 1 caractere
                    .HasConversion(
                        v => v.HasValue ? v.ToString() : null, // Converte o enum para string, ou nulo se o enum for nulo
                        v => !string.IsNullOrEmpty(v) ? (SexoEnum)Enum.Parse(typeof(SexoEnum), v) : (SexoEnum?)null // Converte a string de volta para o enum, ou nulo se a string for nula ou vazia
                    );
            });
        }
    }
}
