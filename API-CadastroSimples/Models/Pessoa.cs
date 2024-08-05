using API_CadastroSimples.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_CadastroSimples.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public SexoEnum? Sexo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; } // Deixei como possível null para não preencher com vários zeros quando eu cadastro a pessoa.
        public Guid Codigo { get; set; }

        [JsonIgnore] // Para não serializar no JSON teste do Postman
        [NotMapped]
        // Propriedade calculada, apenas para controle interno da regra de negócio, não armazenada no banco de dados.
        public bool MaiorIdade => Idade >= 18; // Se Idade < 18 bool MaiorIdade = false e não cadastra (apliquei a validação no método da camada Business.
    }
}
