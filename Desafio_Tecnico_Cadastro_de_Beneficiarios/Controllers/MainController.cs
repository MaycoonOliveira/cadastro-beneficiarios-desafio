using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Enums;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected readonly List<string> Erros = new List<string>();

        protected IActionResult CustomResponse<T>(ResponseModel<T> response)
        {
            if (response.Status)
            {
                return Ok(response);
            }

            return response.Tipo switch
            {
                TipoMensagem.ErroValidacao => BadRequest(response),
                TipoMensagem.NaoEncontrado => NotFound(response),
                TipoMensagem.ErroServidor => StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => BadRequest(response)
            };
        }

        protected IActionResult CustomResponse()
        {
            var response = new ResponseModel<object>
            {
                Status = false,
                Tipo = TipoMensagem.ErroValidacao,
                Mensagem = "Ocorreram um ou mais erros de validação.",
                Details = Erros.Select(e => new ErroValidacaoModel { Mensagem = e }).ToList()
            };
            return BadRequest(response);
        }

        protected IActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }
            return CustomResponse();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            Erros.Add(erro);
        }
    }
}