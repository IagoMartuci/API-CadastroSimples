using API_CadastroSimples.Models;

namespace API_CadastroSimples.Repository
{
    public interface IPessoasRepository
    {
        Task<IEnumerable<Pessoa>> GetAllRepositoryAsync();
        Task<Pessoa> GetByIdRepositoryAsync(int id);
        Task<IEnumerable<Pessoa>> GetByNomeRepositoryAsync(string nome);
        Task<Pessoa> CadastrarPessoaRepositoryAsync(Pessoa pessoa);
        //Task<Pessoa> AlterarCadastroPessoaRepositoryAsync(Pessoa pessoa);
        //Task<int> DeletarCadastroPessoaRepositoryAsync(int id);
        Task<Pessoa> BuscarPorNomeRepositoryAsync(string nome);
    }
}
