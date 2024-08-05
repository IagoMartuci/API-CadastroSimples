using API_CadastroSimples.Business;
using API_CadastroSimples.Models;
using API_CadastroSimples.Repository;

namespace API_CadastroSimples.Service.Implementations
{
    public class PessoasService : IPessoasService
    {
        private readonly IPessoasRepository _pessoasRepository;
        private readonly IPessoasBusiness _pessoasBusiness;
        private readonly ILogger<PessoasService> _logger;

        public PessoasService(IPessoasRepository pessoasRepository, IPessoasBusiness pessoasBusiness, ILogger<PessoasService> logger)
        {
            _pessoasRepository = pessoasRepository;
            _pessoasBusiness = pessoasBusiness;
            _logger = logger;
        }

        public async Task<IEnumerable<Pessoa>> GetAllServiceAsync()
        {
            try
            {
                return await _pessoasRepository.GetAllRepositoryAsync();
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
                return await _pessoasRepository.GetByIdRepositoryAsync(id);
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
                return await _pessoasRepository.GetByNomeRepositoryAsync(nome);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o cadastro com o NOME: {Nome} - Service (Exception).", nome);
                throw;
            }
        }

        public async Task<Pessoa> CadastrarPessoaServiceAsync(Pessoa pessoa)
        {
            try
            {
                var pessoaValidada = await _pessoasBusiness.CadastrarPessoaBusinessAsync(pessoa);
                return await _pessoasRepository.CadastrarPessoaRepositoryAsync(pessoaValidada);
            }
            //catch (BadHttpRequestException brEx)
            //{
            //    _logger.LogWarning(brEx, "Erro ao efetuar o cadastro - Service (BadHttpRequestException).");
            //    throw;
            //}
            catch (InvalidOperationException ioEx)
            {
                _logger.LogWarning(ioEx, "Erro ao efetuar o cadastro - Service (InvalidOperationException).");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao efetuar o cadastro - Service (Exception).");
                throw;
            }
        }

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
