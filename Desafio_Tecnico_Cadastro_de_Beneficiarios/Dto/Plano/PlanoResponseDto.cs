using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using System.Collections.Generic;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano
{
    public class PlanoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Codigo_registro_ans { get; set; }
        public List<BeneficiarioResponseDto> Beneficiarios { get; set; } = new();
    }
}