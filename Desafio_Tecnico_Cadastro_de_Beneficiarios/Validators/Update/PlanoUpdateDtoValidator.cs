using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using FluentValidation;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Validators.Update
{
    public class PlanoUpdateDtoValidator : AbstractValidator<PlanoEdicaoDto>
    {
        public PlanoUpdateDtoValidator()
        {
            RuleFor(x => x.Codigo_registro_ans).NotEmpty().WithMessage("O código de registro ANS é obrigatório");
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O nome do plano é obrigatório");
        }
    }
}
