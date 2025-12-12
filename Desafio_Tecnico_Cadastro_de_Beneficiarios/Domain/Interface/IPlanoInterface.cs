using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Interface
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