namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Models
{
    public class PlanoModel
    {
        public int Id { get; set; } = 0;
        public string Nome { get; set; } = string.Empty;
        public string Codigo_registro_ans { get; set; } = string.Empty;
        public ICollection<BeneficiarioModel> Beneficiarios { get; set; } = new List<BeneficiarioModel>();

    }
}
