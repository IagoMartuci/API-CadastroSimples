using API_CadastroSimples.Models;
using API_CadastroSimples.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetAllAsync()
        {
            try
            {
                return Ok(await _pessoasService.GetAllServiceAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar a lista de pessoas na controller.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
