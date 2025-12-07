using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Profiles
{
    public class BeneficiarioProfile : Profile
    {
        public BeneficiarioProfile()
        {
            CreateMap<BeneficiarioCriacaoDto, BeneficiarioModel>();
            CreateMap<BeneficiarioEdicaoDto, BeneficiarioModel>();
            CreateMap<BeneficiarioModel, BeneficiarioEdicaoDto>();

            CreateMap<BeneficiarioModel, BeneficiarioResponseDto>();
        }
    }
}