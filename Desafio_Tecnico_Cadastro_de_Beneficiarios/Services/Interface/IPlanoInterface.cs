using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface
{
    public interface IPlanoInterface
    {
        Task<ResponseModel<List<PlanoResponseDto>>> GetAllAsync(PlanoFiltroDto planoFiltroDto);
        Task<ResponseModel<PlanoResponseDto>> GetByIdAsync(int id);
        Task<ResponseModel<PlanoResponseDto>> CreateAsync(PlanoCriacaoDto planoCriacaoDto);
        Task<ResponseModel<PlanoResponseDto>> UpdateAsync(PlanoEdicaoDto planoEdicaoDto);
        Task<ResponseModel<PlanoModel>> DeletarPlano(int id);
    }
}