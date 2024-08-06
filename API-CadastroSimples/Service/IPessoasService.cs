using API_CadastroSimples.Models;

namespace API_CadastroSimples.Service
{
    public interface IPessoasService
    {
        Task<IEnumerable<Pessoa>> GetAllServiceAsync();
        Task<Pessoa> GetByIdServiceAsync(int id);
        Task<IEnumerable<Pessoa>> GetByNomeServiceAsync(string nome);
        Task<Pessoa> CadastrarPessoaServiceAsync(Pessoa pessoa);
        Task<Pessoa> AlterarCadastroPessoaServiceAsync(Pessoa pessoa);
        //Task<int> DeletarCadastroPessoaServiceAsync(int id);
    }
}
