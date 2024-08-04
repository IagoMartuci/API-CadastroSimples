using API_CadastroSimples.Models;
using API_CadastroSimples.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace API_CadastroSimples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoasService _pessoasService;
        private readonly ILogger<PessoasController> _logger;

        public PessoasController(IPessoasService pessoasService, ILogger<PessoasController> logger)
        {
            _pessoasService = pessoasService;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetAllAsync()
        {
            try
            {
                //return Ok(await _pessoasService.GetAllServiceAsync()); // Retornando a lista direto, se não tiver item retorna a lsita vazia.

                var result = await _pessoasService.GetAllServiceAsync();

                if (!result.Any()) // Configurando para ao invés de retornar a lista vazia, retornar uma mensagem personalizada.
                {
                    _logger.LogInformation("Nenhum cadastro encontrado - Controller.");
                    return NotFound("Nenhum cadastro encontrado.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar a lista de pessoas - Controller.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            try
            {
                return Ok(await _pessoasService.GetByIdServiceAsync(id));
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Nenhum cadastro encontrado com o ID: {Id} - Controller (KeyNotFoundException).", id);
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nenhum cadastro encontrado com o ID: {Id} - Controller (Exception).", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetByNomeAsync(string nome)
        {
            try
            {
                var result = await _pessoasService.GetByNomeServiceAsync(nome);

                if (!result.Any())
                {
                    _logger.LogInformation("Nenhum cadastro encontrado com o NOME: {Nome} - Controller.", nome);
                    return NotFound($"Nenhum cadastro encontrado com o NOME: {nome}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastro não encontrado com o NOME: {Nome} - Controller (Exception).", nome);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
            }
        }

    }
}

