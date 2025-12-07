using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Validators.Create;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Validators.Filter;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Validators.Update;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanoController : MainController
    {
        private readonly IPlanoInterface _planoInterface;

        public PlanoController(IPlanoInterface planoInterface)
        {
            _planoInterface = planoInterface;
        }

        /// <summary>
        /// Buscar Planos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarPlanos([FromQuery] PlanoFiltroDto planoFiltroDto)
        {

            var validationResult = await new PlanoFiltroDtoValidator().ValidateAsync(planoFiltroDto);
            if (!validationResult.IsValid)
                return CustomResponse(validationResult);

            var response = await _planoInterface.GetAllAsync(planoFiltroDto);
            return CustomResponse(response);
        }

        /// <summary>
        /// Buscar plano ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarPlanosId(int id)
        {
            var response = await _planoInterface.GetByIdAsync(id);
            return CustomResponse(response);
        }

        /// <summary>
        /// Criar Plano
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarPlano([FromBody] PlanoCriacaoDto planoCriacaoDto)
        {
            var validationResult = await new PlanoCreatDtoValidator().ValidateAsync(planoCriacaoDto);

            if (!validationResult.IsValid) 
                return CustomResponse(validationResult);

            var response = await _planoInterface.CreateAsync(planoCriacaoDto);

            if (!response.Status)
                return CustomResponse(response);

            return CreatedAtAction(nameof(BuscarPlanosId), new { id = response.Dados.Id }, response);
        }

        /// <summary>
        /// Edita um plano existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditarPlano(int id, [FromBody] PlanoEdicaoDto planoEdicaoDto)
        {
            if (planoEdicaoDto == null)
            {
                AdicionarErroProcessamento("Corpo da requisição ausente.");
                return CustomResponse();
            }

            if (planoEdicaoDto.Id != 0 && planoEdicaoDto.Id != id)
            {
                AdicionarErroProcessamento("O id da rota deve ser igual ao id informado no corpo da requisição.");
                return CustomResponse();
            }

            planoEdicaoDto.Id = id;

            var validationResult = await new PlanoUpdateDtoValidator().ValidateAsync(planoEdicaoDto);
            if (!validationResult.IsValid)
                return CustomResponse(validationResult);

            var response = await _planoInterface.UpdateAsync(planoEdicaoDto);

            return CustomResponse(response);
        }

        /// <summary>
        /// Deleta um plano pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarPlano(int id)
        {
            var response = await _planoInterface.DeletarPlano(id);
            return CustomResponse(response);
        }
    }
}
