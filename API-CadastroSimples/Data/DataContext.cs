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
                    .HasColumnType("char(1)") // Define o tipo da coluna como char com comprimento de 1 caractere
                    .HasConversion(
                        v => v.HasValue ? v.ToString()[0] : (char?)null, // Converte o enum para char, ou nulo se o enum for nulo
                        v => v.HasValue ? (SexoEnum)Enum.ToObject(typeof(SexoEnum), v) : (SexoEnum?)null // Converte o char de volta para o enum, ou nulo se o char for null
                    );

                entity.Property(p => p.DataCadastro)
                    .HasColumnType("varchar(100)"); // Define o campo DataCadastro como um varchar com tamanho máximo de 100 caracteres
            });
        }

    }
}
