using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Validators.Create;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Validators.Update;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiarioController : MainController
    {
        private readonly IBeneficiarioInterface _beneficiarioInterface;

        public BeneficiarioController(IBeneficiarioInterface beneficiarioInterface)
        {
            _beneficiarioInterface = beneficiarioInterface;
        }

        /// <summary>
        /// Lista todos os beneficiários
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarBeneficiarios([FromQuery] BeneficiarioFiltroDto filtroDto)
        {
            var response = await _beneficiarioInterface.GetAllAsync(filtroDto);
            return CustomResponse(response);
        }

        /// <summary>
        /// Retorna um beneficiário pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Detalhe(int id)
        {
            var response = await _beneficiarioInterface.GetByIdAsync(id);
            return CustomResponse(response);
        }

        /// <summary>
        /// Criar Beneficiário
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CriarBeneficiario([FromBody] BeneficiarioCriacaoDto beneficiarioCriacaoDto)
        {
            var validationResult = await new BeneficiarioCreatDtoValidator().ValidateAsync(beneficiarioCriacaoDto);
            if (!validationResult.IsValid)
                return CustomResponse(validationResult);

            var response = await _beneficiarioInterface.CreateAsync(beneficiarioCriacaoDto);

            if (!response.Status)
                return CustomResponse(response);

            return CreatedAtAction(nameof(Detalhe), new { id = response.Dados.Id }, response);
        }

        /// <summary>
        /// Edita um beneficiário existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditarBeneficiario(int id, [FromBody] BeneficiarioEdicaoDto dto)
        {
            if (dto == null)
            {
                AdicionarErroProcessamento("Corpo da requisição ausente.");
                return CustomResponse();
            }

            if (dto.Id != 0 && dto.Id != id)
            {
                AdicionarErroProcessamento("O id da rota deve ser igual ao id informado no corpo da requisição.");
                return CustomResponse();
            }
            dto.Id = id;

            var validationResult = await new BeneficiarioUpdateDtoValidator().ValidateAsync(dto);
            if (!validationResult.IsValid)
                return CustomResponse(validationResult);

            var response = await _beneficiarioInterface.UpdateAsync(dto);
            return CustomResponse(response);
        }

        /// <summary>
        /// Deleta um beneficiário pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletarBeneficiario(int id, [FromQuery] int prioridade = 3)
        {
            var response = await _beneficiarioInterface.DeleteAsync(id, prioridade);

            if (!response.Status)
                return CustomResponse(response);

            return CustomResponse(response);
        }
    }
}