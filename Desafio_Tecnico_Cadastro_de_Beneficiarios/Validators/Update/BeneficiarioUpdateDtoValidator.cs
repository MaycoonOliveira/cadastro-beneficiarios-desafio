using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Utils;
using FluentValidation;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Validators.Update
{
    public class BeneficiarioUpdateDtoValidator : AbstractValidator<BeneficiarioEdicaoDto>
    {
        public BeneficiarioUpdateDtoValidator()
        {
            RuleFor(x => x.NomeCompleto)
                .NotEmpty().WithMessage("O nome completo é obrigatório")
                .MaximumLength(150).WithMessage("Nome muito longo");

            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("O CPF é obrigatório")
                .Must(CpfValidator.IsValid).WithMessage("CPF inválido.");

            RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória")
                .LessThan(DateTime.Today).WithMessage("A data de nascimento deve ser anterior a hoje.");

            RuleFor(x => x.PlanoId)
                .NotEmpty().WithMessage("O plano é obrigatório.")
                .GreaterThan(0).WithMessage("O ID do plano é inválido.");
        }
    }
}
