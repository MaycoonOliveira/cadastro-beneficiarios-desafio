using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Beneficiario
{
    public class BeneficiarioCriacaoDto
    {

        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public Status Status { get; set; }
        public int PlanoId { get; set; }
    }
}
