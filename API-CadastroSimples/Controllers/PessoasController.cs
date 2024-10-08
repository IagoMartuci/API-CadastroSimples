﻿using API_CadastroSimples.Models;
using API_CadastroSimples.Service;
using Microsoft.AspNetCore.Mvc;

namespace API_CadastroSimples.Controllers
{
    [Route("api/[controller]/")]
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
                var result = await _pessoasService.GetAllServiceAsync();

                if (!result.Any()) // Configurando para ao invés de retornar a lista vazia, retornar uma mensagem personalizada.
                {
                    _logger.LogInformation("Nenhum cadastro encontrado - Controller.");
                    return NotFound("Nenhum cadastro encontrado - Controller.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar a lista de pessoas - Controller (Exception).");
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
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetByNomeAproximadoAsync(string nome)
        {
            try
            {
                var result = await _pessoasService.GetByNomeAproximadoServiceAsync(nome);

                if (!result.Any())
                {
                    _logger.LogInformation("Nenhum cadastro encontrado com o NOME: {Nome} - Controller.", nome);
                    return NotFound($"Nenhum cadastro encontrado com o NOME: \"{nome}\" - Controller.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cadastro não encontrado com o NOME: {Nome} - Controller (Exception).", nome);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CadastrarPessoaAsync(Pessoa pessoa)
        {
            try
            {
                var pessoaCadastrada = await _pessoasService.CadastrarPessoaServiceAsync(pessoa);
                var url = Url.Action(nameof(GetByIdAsync), new { id = pessoaCadastrada.Id });
                //return Created(url, pessoaCadastrada.Id);
                //return Created(url, pessoaCadastrada);
                //return CreatedAtAction(url, pessoaCadastrada.Id); // Retorna somente o id
                return CreatedAtAction(url, pessoaCadastrada); // Retorna o objeto todo

            }
            // Optei por utilizar o InvalidOperationException, mas este também funciona:
            //catch (BadHttpRequestException brEx)
            //{
            //    _logger.LogWarning(brEx, "Erro ao efetuar o cadastro - Controller (BadHttpRequestException).");
            //    return BadRequest(brEx.Message);
            //}
            catch (InvalidOperationException ioEx)
            {
                _logger.LogWarning(ioEx, "Erro ao efetuar o cadastro - Controller (InvalidOperationException).");
                return BadRequest(ioEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao efetuar o cadastro - Controller (Exception).");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> AlterarPessoaAsync(Pessoa pessoa)
        {
            try
            {
                return Ok(await _pessoasService.AlterarCadastroPessoaServiceAsync(pessoa));
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Erro ao buscar o cadastro com o ID: {Id} - Controller (KeyNotFoundException).", pessoa.Id);
                return NotFound(knfEx.Message);
            }
            // Quando eu tentava desconectar o BD e fazer a requisição, estava retornando HTTP 400 ao invés de HTTP 500:
            // Tive que trocar a InvalidOperationException pela BadHttpRequestException, pois a InvalidOperationException é mais genérica do que a BadHttpRequestException,
            // então a Exception que acontecia no método GetById do Repository estava retornando como InvalidOperationException nos metodos AlterarCadastro das camadas superiores.
            // Portando, ela chegava na Controller e caia na InvalidOperationException (HTTP 400) ao invés de cair na Exception (HTTP 500). Com a BadHttpRequestException, por ser mais específica não acontece isso.
            // Obs.: método AlterarCadastroPessoaBusinessAsync utiliza o método GetByIdRepositoryAsync para validar o ID, por isso a Exception é gerada originalmente no método GetByIdRepositoryAsync.
            catch (BadHttpRequestException brEx)
            {
                _logger.LogWarning(brEx, "Erro ao atualizar o cadastro - Controller (BadHttpRequestException).");
                return BadRequest(brEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o cadastro - Controller (Exception).");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCadastroPessoaAsync(int id)
        {
            try
            {
                return Ok(await _pessoasService.DeletarCadastroPessoaServiceAsync(id));
            }
            catch (KeyNotFoundException knfEx)
            {
                _logger.LogWarning(knfEx, "Erro ao buscar o cadastro com o ID: {Id} - Controller (KeyNotFoundException).", id);
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o cadastro - Controller (Exception).");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno.");
            }
        }
    }
}

