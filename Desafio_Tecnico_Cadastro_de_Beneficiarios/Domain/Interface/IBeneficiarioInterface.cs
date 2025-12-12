using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Interface
{
    public interface IBeneficiarioInterface
    {
        Task<ResponseModel<List<BeneficiarioResponseDto>>> GetAllAsync(BeneficiarioFiltroDto filtroDto);
        Task<ResponseModel<BeneficiarioResponseDto>> GetByIdAsync(int id);
        Task<ResponseModel<BeneficiarioResponseDto>> CreateAsync(BeneficiarioCriacaoDto beneficiarioCriacaoDto);
        Task<ResponseModel<BeneficiarioResponseDto>> UpdateAsync(BeneficiarioEdicaoDto beneficiarioEdicaoDto);
        Task<ResponseModel<BeneficiarioModel>> DeleteAsync(int id, int prioridade);
    }
}
