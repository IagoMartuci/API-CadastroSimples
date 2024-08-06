using API_CadastroSimples.Data;
using API_CadastroSimples.Models;
using Microsoft.Data.SqlClient;
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
                var result = await _context.Pessoas.ToListAsync();

                if (!result.Any()) // Configurando para ao invés de retornar a lista vazia, retornar uma mensagem personalizada.
                {
                    _logger.LogInformation("Nenhum cadastro encontrado - Repository.");
                    // Retorne uma lista vazia ao invés de lançar uma exceção
                    return Enumerable.Empty<Pessoa>();
                }

                return result;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Erro ao recuperar a lista de pessoas - Repository (DbUpdateException).");
                throw;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro ao recuperar a lista de pessoas - Repository (SqlException).");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar a lista de pessoas - Repository (Exception).");
                throw;
            }
        }

        public async Task<Pessoa> GetByIdRepositoryAsync(int id)
        {
            try
            {
                var result = await _context.Pessoas
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (result == null)
                {
                    throw new KeyNotFoundException($"Nenhum cadastro encontrado com o ID: {id}");
                }

                return result;
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Erro ao buscar o cadastro com o ID: {Id} - Repository (KeyNotFoundException).", id);
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Erro ao buscar o cadastro com o ID: {Id} - Repository (DbUpdateException).", id);
                throw;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro ao buscar o cadastro com o ID: {Id} - Repository (SqlException).", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o cadastro com o ID: {Id} - Repository (Exception).", id);
                throw;
            }
        }

        public async Task<IEnumerable<Pessoa>> GetByNomeRepositoryAsync(string nome)
        {
            try
            {
                var result = await _context.Pessoas
                    .Where(x => x.Nome.ToLower().Contains(nome.ToLower()))
                    .ToListAsync();

                if (!result.Any())
                {
                    _logger.LogInformation("Nenhum cadastro encontrado com o NOME: {Nome} - Repository.", nome);
                    // Retorne uma lista vazia ao invés de lançar uma exceção
                    return Enumerable.Empty<Pessoa>();
                }

                return result;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Erro ao buscar o cadastro com o NOME: {Nome} - Repository (DbUpdateException).", nome);
                throw;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro ao buscar o cadastro com o NOME: {Nome} - Repository (SqlException).", nome);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o cadastro com o NOME: {Nome} - Repository (Exception).", nome);
                throw;
            }
        }

        public async Task<Pessoa> CadastrarPessoaRepositoryAsync(Pessoa pessoa)
        {
            try
            {
                pessoa.DataCadastro = DateTime.Now;
                pessoa.Codigo = Guid.NewGuid();

                await _context.Pessoas.AddAsync(pessoa);
                await _context.SaveChangesAsync();

                return pessoa;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Erro ao efetuar o cadastro - Repository (DbUpdateException).");
                throw;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro ao efetuar o cadastro - Repository (SqlException).");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao efetuar o cadastro - Repository (Exception).");
                throw;
            }
        }

        public async Task<Pessoa> AlterarCadastroPessoaRepositoryAsync(Pessoa pessoa)
        {
            try
            {
                // Busca a entidade existente pelo ID
                var result = await _context.Pessoas.SingleOrDefaultAsync(x => x.Id == pessoa.Id);

                // Se a entidade não for encontrada, lance uma exceção ou retorne um resultado apropriado
                if (result == null)
                {
                    throw new KeyNotFoundException("Pessoa não encontrada - Repository.");
                }

                // Atualiza as propriedades específicas
                var entry = _context.Entry(result);

                //Apenas marque as propriedades que você deseja modificar
                if (pessoa.Nome != null)
                {
                    entry.Property(e => e.Nome).CurrentValue = pessoa.Nome;
                    entry.Property(e => e.Nome).IsModified = true;
                }
                if (pessoa.Idade != default)
                {
                    entry.Property(e => e.Idade).CurrentValue = pessoa.Idade;
                    entry.Property(e => e.Idade).IsModified = true;
                }
                if (pessoa.Sexo != null || entry.Property(e => e.Sexo).CurrentValue != null)
                {
                    entry.Property(e => e.Sexo).CurrentValue = pessoa.Sexo;
                    entry.Property(e => e.Sexo).IsModified = true;
                }

                // Atualize a data de alteração
                entry.Property(e => e.DataAlteracao).CurrentValue = DateTime.Now;
                entry.Property(e => e.DataAlteracao).IsModified = true;

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();

                return result;
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Erro ao buscar o cadastro com o ID: {Id} - Repository (KeyNotFoundException).", pessoa.Id);
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Erro ao atualizar o cadastro - Repository (DbUpdateException).");
                throw;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Erro ao atualizar o cadastro - Repository (SqlException).");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o cadastro - Repository (Exception).");
                throw;
            }
        }

        //public Task<int> DeletarCadastroPessoaRepositoryAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Pessoa> BuscarPorNomeRepositoryAsync(string nome)
        {
            try
            {
                return await _context.Pessoas
                    .FirstOrDefaultAsync(x => x.Nome.ToLower() == nome.ToLower());
            }
            // Capturar a InvalidOperationException e lançar uma Exception comum
            // em vez de simplesmente relançar a InvalidOperationException.
            catch (InvalidOperationException ioEx)
            {
                // Lança Exception em vez de InvalidOperationException para a camada superior, dessa forma eu consigo ir lançando como Exception até a controller, e retornar o código 500
                throw new Exception(ioEx.Message, ioEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar nome no banco de dados - Repository (Exception).");
                throw;
            }
        }
    }
}
