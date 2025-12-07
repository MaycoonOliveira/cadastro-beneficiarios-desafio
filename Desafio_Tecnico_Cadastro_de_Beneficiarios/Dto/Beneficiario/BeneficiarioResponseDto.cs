using Desafio_Tecnico_Cadastro_de_Beneficiarios.Enum;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario
{
    public class BeneficiarioResponseDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public Status Status { get; set; }
        public int PlanoId { get; set; }
    }
}