using API_CadastroSimples.Models;

namespace API_CadastroSimples.Business
{
    public interface IPessoasBusiness
    {
        Task<Pessoa> CadastrarPessoaBusinessAsync(Pessoa pessoa);
        Task<Pessoa> AlterarCadastroPessoaBusinessAsync(Pessoa pessoa);
    }
}
