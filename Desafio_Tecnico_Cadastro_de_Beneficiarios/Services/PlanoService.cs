using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Data;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Enum;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Services
{
    public class PlanoService : IPlanoInterface
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlanoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<PlanoResponseDto>>> GetAllAsync(PlanoFiltroDto planoFiltroDto)
        {
            var response = new ResponseModel<List<PlanoResponseDto>>();
            try
            {
                var query = _context.Planos
                    .Include(p => p.Beneficiarios)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(planoFiltroDto.Codigo_registro_ans))
                {
                    query = query.Where(p => p.Codigo_registro_ans.Contains(planoFiltroDto.Codigo_registro_ans.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(planoFiltroDto.Nome))
                {
                    query = query.Where(p => p.Nome.Contains(planoFiltroDto.Nome.Trim()));
                }

                var planosDb = await query.ToListAsync();
                response.Dados = _mapper.Map<List<PlanoResponseDto>>(planosDb);
                response.Mensagem = !response.Dados.Any() ? "Nenhum plano encontrado" : "Planos listados com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao buscar os planos: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<PlanoResponseDto>> GetByIdAsync(int id)
        {
            var response = new ResponseModel<PlanoResponseDto>();
            try
            {
                var plano = await _context.Planos
                    .Include(p => p.Beneficiarios)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (plano == null)
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.NaoEncontrado;
                    response.Mensagem = "Plano não localizado";
                    return response;
                }

                response.Dados = _mapper.Map<PlanoResponseDto>(plano);
                response.Mensagem = "Plano localizado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao buscar o plano: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<PlanoResponseDto>> CreateAsync(PlanoCriacaoDto planoCriacaoDto)
        {
            var response = new ResponseModel<PlanoResponseDto>();
            try
            {
                var existing = await _context.Planos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Codigo_registro_ans == planoCriacaoDto.Codigo_registro_ans);

                if (existing != null)
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.ErroValidacao;
                    response.Mensagem = "Um plano com este código de registro ANS já existe.";
                    response.Details.Add(new ErroValidacaoModel { Id = existing.Id, Campo = "Codigo_registro_ans" });
                    return response;
                }

                var plano = _mapper.Map<PlanoModel>(planoCriacaoDto);

                await _context.AddAsync(plano);
                await _context.SaveChangesAsync();

                response.Dados = _mapper.Map<PlanoResponseDto>(plano);
                response.Mensagem = "Plano criado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao criar o plano: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<PlanoResponseDto>> UpdateAsync(PlanoEdicaoDto planoEdicaoDto)
        {
            var response = new ResponseModel<PlanoResponseDto>();
            try
            {
                var planoBanco = await _context.Planos
                    .Include(p => p.Beneficiarios)
                    .FirstOrDefaultAsync(p => p.Id == planoEdicaoDto.Id);

                if (planoBanco == null)
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.NaoEncontrado;
                    response.Mensagem = "Plano não localizado para edição.";
                    return response;
                }

                if (await _context.Planos.AnyAsync(p => p.Codigo_registro_ans == planoEdicaoDto.Codigo_registro_ans && p.Id != planoEdicaoDto.Id))
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.ErroValidacao;
                    response.Mensagem = "O código de registro ANS informado já está em uso por outro plano.";
                    return response;
                }

                _mapper.Map(planoEdicaoDto, planoBanco);

                await _context.SaveChangesAsync();

                response.Dados = _mapper.Map<PlanoResponseDto>(planoBanco);
                response.Mensagem = "Plano editado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao editar o plano: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<PlanoModel>> DeletarPlano(int id)
        {
            var response = new ResponseModel<PlanoModel>();
            try
            {
                var plano = await _context.Planos.Include(p => p.Beneficiarios).FirstOrDefaultAsync(p => p.Id == id);

                if (plano == null)
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.NaoEncontrado;
                    response.Mensagem = "Plano não localizado";
                    return response;
                }

                if (plano.Beneficiarios.Any())
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.ErroValidacao;
                    response.Mensagem = "Não é possível deletar um plano que possui beneficiários associados.";
                    return response;
                }

                _context.Planos.Remove(plano);
                await _context.SaveChangesAsync();

                response.Dados = plano; 
                response.Mensagem = "Plano removido com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao deletar o plano: " + ex.Message;
                return response;
            }
        }
    }
}