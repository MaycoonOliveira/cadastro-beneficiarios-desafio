using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Plano;
using FluentValidation;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Validators.Filter
{
    public class PlanoFiltroDtoValidator : AbstractValidator<PlanoFiltroDto>
    {
        public PlanoFiltroDtoValidator()
        {
            RuleFor(x => x.Nome).MaximumLength(20).WithMessage("O nome pode ter no máximo 20 caracteres.");
            RuleFor(x => x.Codigo_registro_ans).MaximumLength(20).WithMessage("O Codigo de registro ANS pode ter no máximo 20 caracteres.");
        }
    }
}



