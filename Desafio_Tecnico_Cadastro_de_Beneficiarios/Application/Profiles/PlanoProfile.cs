using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Profiles
{
    public class PlanoProfile : Profile
    {
        public PlanoProfile()
        {
            CreateMap<PlanoCriacaoDto, PlanoModel>();
            CreateMap<PlanoEdicaoDto, PlanoModel>();
            CreateMap<PlanoModel, PlanoEdicaoDto>();

            CreateMap<PlanoModel, PlanoResponseDto>()
                .ForMember(dest => dest.Beneficiarios, opt => opt.MapFrom(src => src.Beneficiarios));

            CreateMap<PlanoModel, PlanoSimplesDto>();
        }
    }
}