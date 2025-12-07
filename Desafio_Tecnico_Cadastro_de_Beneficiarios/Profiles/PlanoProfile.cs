using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Profiles
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
        }
    }
}