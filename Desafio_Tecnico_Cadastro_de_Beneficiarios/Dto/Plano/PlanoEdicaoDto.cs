using System.ComponentModel.DataAnnotations;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano
{
    public class PlanoEdicaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo_registro_ans { get; set; } = string.Empty;
    }
}
