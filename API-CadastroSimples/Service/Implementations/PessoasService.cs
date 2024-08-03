using API_CadastroSimples.Models;
using API_CadastroSimples.Repository;
using API_CadastroSimples.Repository.Implementations;

namespace API_CadastroSimples.Service.Implementations
{
    public class PessoasService : IPessoasService
    {
        private readonly IPessoasRepository _pessoaRepository;
        private readonly ILogger<PessoasService> _logger;

        public PessoasService(IPessoasRepository pessoaRepository, ILogger<PessoasService> logger)
        {
            _pessoaRepository = pessoaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Pessoa>> GetAllServiceAsync()
        {
            try
            {
                return await _pessoaRepository.GetAllRepositoryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar a lista de pessoas da camada de serviço.");
                throw;
            }
        }

        public Task<Pessoa> GetByIdServiceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Pessoa> GetByNomeServiceAsync(string nome)
        {
            throw new NotImplementedException();
        }

        public Task<Pessoa> CadastrarPessoaServiceAsync(Pessoa pessoa)
        {
            throw new NotImplementedException();
        }

        public Task<Pessoa> AlterarCadastroPessoaServiceAsync(Pessoa pessoa)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeletarCadastroPessoaServiceAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
