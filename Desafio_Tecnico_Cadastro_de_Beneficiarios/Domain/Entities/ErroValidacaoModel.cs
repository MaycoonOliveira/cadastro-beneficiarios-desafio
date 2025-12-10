namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities
{
    public class ErroValidacaoModel
    {
        public int? Id { get; set; }
        public string? Campo { get; set; } = string.Empty;
        public string? Mensagem { get; set; } = string.Empty;
    }
}
