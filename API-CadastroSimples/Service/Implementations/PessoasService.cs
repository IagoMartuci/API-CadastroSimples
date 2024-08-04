using API_CadastroSimples.Models;
using API_CadastroSimples.Repository;

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
                _logger.LogError(ex, "Erro ao recuperar a lista de pessoas - Service (Exception).");
                throw;
            }
        }

        public async Task<Pessoa> GetByIdServiceAsync(int id)
        {
            try
            {
                return await _pessoaRepository.GetByIdRepositoryAsync(id);
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Nenhum cadastro encontrado com o ID: {Id} - Service (KeyNotFoundException).", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o cadastro com o ID: {Id} - Service (Exception).", id);
                throw;
            }
        }

        public async Task<IEnumerable<Pessoa>> GetByNomeServiceAsync(string nome)
        {
            try
            {
                return await _pessoaRepository.GetByNomeRepositoryAsync(nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o cadastro com o NOME: {Nome} - Service (Exception).", nome);
                throw;
            }
        }

        //public Task<Pessoa> CadastrarPessoaServiceAsync(Pessoa pessoa)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Pessoa> AlterarCadastroPessoaServiceAsync(Pessoa pessoa)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<int> DeletarCadastroPessoaServiceAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
