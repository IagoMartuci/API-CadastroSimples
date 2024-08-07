using API_CadastroSimples.Models;
using API_CadastroSimples.Models.Enums;
using API_CadastroSimples.Repository;

namespace API_CadastroSimples.Business.Implementations
{
    public class PessoasBusiness : IPessoasBusiness
    {
        private readonly IPessoasRepository _pessoasRepository;
        private readonly ILogger<PessoasBusiness> _logger;

        public PessoasBusiness(IPessoasRepository pessoasRepository,  ILogger<PessoasBusiness> logger)
        {
            _pessoasRepository = pessoasRepository;
            _logger = logger;
        }

        public async Task<Pessoa> CadastrarPessoaBusinessAsync(Pessoa pessoa)
        {
            try
            {
                var pessoaComNomeExistente = await _pessoasRepository.BuscarPorNomeRepositoryAsync(pessoa.Nome);

                if (pessoaComNomeExistente != null)
                {
                    throw new InvalidOperationException($"Já existe uma pessoa cadastrada com o NOME: {pessoa.Nome} no BD: {pessoaComNomeExistente.Nome} - Business");
                }
                if (!pessoa.MaiorIdade)
                {
                    throw new InvalidOperationException($"A idade mínima para cadastro é 18 anos - Business.");
                }
                if (!pessoa.Sexo.Equals("F") && !pessoa.Sexo.Equals("M")
                        && pessoa.Sexo != SexoEnum.F && pessoa.Sexo != SexoEnum.M
                            && pessoa.Sexo != null)
                {
                    throw new InvalidOperationException($"Sexo deve ser F, M ou null - Business.");
                }

                return pessoa;
            }
            // Capturar a InvalidOperationException e lançar uma BadHttpRequestException
            // em vez de simplesmente relançar a InvalidOperationException,
            // é útil se eu quiser retornar BadHttpRequestException na controller.
            //catch (InvalidOperationException ioEx)
            //{
            //    // Lança BadHttpRequestException em vez de InvalidOperationException
            //    throw new BadHttpRequestException(ioEx.Message, ioEx);
            //}
            catch (InvalidOperationException ioEx)
            {
                _logger.LogWarning(ioEx, "Erro ao efetuar o cadastro - Business (InvalidOperationException).");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao efetuar o cadastro - Business (Exception).");
                throw;
            }
        }

        public async Task<Pessoa> AlterarCadastroPessoaBusinessAsync(Pessoa pessoa)
        {
            try
            {
                await _pessoasRepository.GetByIdRepositoryAsync(pessoa.Id);

                var pessoaComNomeExistente = await _pessoasRepository.BuscarPorNomeRepositoryAsync(pessoa.Nome);

                if (pessoaComNomeExistente != null && pessoaComNomeExistente.Id != pessoa.Id)
                {
                    throw new BadHttpRequestException($"Já existe uma pessoa cadastrada com o NOME: {pessoa.Nome} no BD: {pessoaComNomeExistente.Nome} - Business");
                }
                if (!pessoa.MaiorIdade)
                {
                    throw new BadHttpRequestException($"A idade mínima para cadastro é 18 anos - Business.");
                }
                if (!pessoa.Sexo.Equals("F") && !pessoa.Sexo.Equals("M")
                        && pessoa.Sexo != SexoEnum.F && pessoa.Sexo != SexoEnum.M
                            && pessoa.Sexo != null)
                {
                    throw new BadHttpRequestException($"Sexo deve ser F, M ou null - Business.");
                }

                return pessoa;
            }
            catch (BadHttpRequestException brEx)
            {
                _logger.LogWarning(brEx, "Erro ao atualizar o cadastro - Business (BadHttpRequestException).");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o cadastro - Business (Exception).");
                throw;
            }
        }
    }
}
