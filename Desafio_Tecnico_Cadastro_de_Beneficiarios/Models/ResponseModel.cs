using Desafio_Tecnico_Cadastro_de_Beneficiarios.Enum;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Models
{
    public class ResponseModel<T>
    {
        public T Dados { get; set; }
        public string Error { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public bool Status { get; set; } = true;

        public TipoMensagem Tipo { get; set; } = TipoMensagem.Sucesso;

        public List<ErroValidacaoModel> Details { get; set; } = new List<ErroValidacaoModel>();
    }
}
