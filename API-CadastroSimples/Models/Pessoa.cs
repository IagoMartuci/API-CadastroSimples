using API_CadastroSimples.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_CadastroSimples.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public SexoEnum? Sexo { get; set; }        
        [NotMapped]
        public bool MaiorIdade => Idade >= 18; // Propriedade calculada, não armazenada no banco de dados
        public string DataCadastro { get; set; }
    }
}
