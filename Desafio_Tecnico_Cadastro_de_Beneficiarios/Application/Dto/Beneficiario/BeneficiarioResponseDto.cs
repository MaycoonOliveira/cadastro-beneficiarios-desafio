using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Enums;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Beneficiario
{
    public class BeneficiarioResponseDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public Status Status { get; set; }
        public int PlanoId { get; set; }
        public PlanoSimplesDto Plano { get; set; }
    }
}