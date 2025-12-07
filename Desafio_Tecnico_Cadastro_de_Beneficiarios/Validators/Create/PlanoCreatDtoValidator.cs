using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using FluentValidation;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Validators.Create
{
    public class PlanoCreatDtoValidator: AbstractValidator<PlanoCriacaoDto>
    {
        public PlanoCreatDtoValidator() 
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("O  nome do plano é obrigatório");
            RuleFor(x => x.Codigo_registro_ans).NotEmpty().WithMessage("O código de registro da ANS é obrigatório");
        }
    }
}
