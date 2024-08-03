using API_CadastroSimples.Data;
using API_CadastroSimples.Models;
using Microsoft.EntityFrameworkCore;

namespace API_CadastroSimples.Repository.Implementations
{
    public class PessoasRepository : IPessoasRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<PessoasRepository> _logger;

        public PessoasRepository(DataContext context, ILogger<PessoasRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Pessoa>> GetAllRepositoryAsync()
        {
            try
            {
                return await _context.Pessoas.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar a lista de pessoas do repositório.");
                throw;
            }
        }

        //public Task<Pessoa> GetByIdRepositoryAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Pessoa> GetByNomeRepositoryAsync(string nome)
        //{
        //    throw new NotImplementedException();
        //}
        //public Task<Pessoa> CadastrarPessoaRepositoryAsync(Pessoa pessoa)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Pessoa> AlterarCadastroPessoaRepositoryAsync(Pessoa pessoa)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<int> DeletarCadastroPessoaRepositoryAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
