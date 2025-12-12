using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Profiles
{
    public class BeneficiarioProfile : Profile
    {
        public BeneficiarioProfile()
        {
            CreateMap<BeneficiarioCriacaoDto, BeneficiarioModel>();
            CreateMap<BeneficiarioEdicaoDto, BeneficiarioModel>();
            CreateMap<BeneficiarioModel, BeneficiarioEdicaoDto>();

            CreateMap<BeneficiarioModel, BeneficiarioResponseDto>()
                .ForMember(dest => dest.Plano, opt => opt.MapFrom(src => src.Plano));
        }
    }
}