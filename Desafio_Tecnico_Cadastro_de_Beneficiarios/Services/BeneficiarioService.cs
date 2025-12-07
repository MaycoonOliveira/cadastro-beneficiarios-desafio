using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Data;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Enum;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Models;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Services
{
    public class BeneficiarioService : IBeneficiarioInterface
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BeneficiarioService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<BeneficiarioResponseDto>>> GetAllAsync(BeneficiarioFiltroDto filtroDto)
        {
            var response = new ResponseModel<List<BeneficiarioResponseDto>>();
            try
            {
                var query = _context.Beneficiarios.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(filtroDto.NomeCompleto))
                {
                    query = query.Where(b => b.NomeCompleto.Contains(filtroDto.NomeCompleto.Trim()));
                }

                if (!string.IsNullOrWhiteSpace(filtroDto.Cpf))
                {
                    var cpfLimpo = new string(filtroDto.Cpf.Where(char.IsDigit).ToArray());
                    query = query.Where(b => b.Cpf.Contains(cpfLimpo));
                }

                var beneficiarios = await query.ToListAsync();
                response.Dados = _mapper.Map<List<BeneficiarioResponseDto>>(beneficiarios);
                response.Mensagem = !response.Dados.Any() ? "Nenhum beneficiário encontrado" : "Beneficiários listados com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao listar os beneficiários: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioResponseDto>> GetByIdAsync(int id)
        {
            var response = new ResponseModel<BeneficiarioResponseDto>();
            try
            {
                var beneficiario = await _context.Beneficiarios.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

                if (beneficiario == null)
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.NaoEncontrado;
                    response.Mensagem = "Beneficiário não localizado";
                    return response;
                }
                response.Dados = _mapper.Map<BeneficiarioResponseDto>(beneficiario);
                response.Mensagem = "Beneficiário localizado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao buscar o beneficiário: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioResponseDto>> CreateAsync(BeneficiarioCriacaoDto beneficiarioCriacaoDto)
        {
            var response = new ResponseModel<BeneficiarioResponseDto>();
            try
            {
                if (await _context.Beneficiarios.AnyAsync(b => b.Cpf == beneficiarioCriacaoDto.Cpf))
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.ErroValidacao;
                    response.Mensagem = "CPF já cadastrado para outro beneficiário.";
                    return response;
                }

                if (!await _context.Planos.AnyAsync(p => p.Id == beneficiarioCriacaoDto.PlanoId))
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.NaoEncontrado;
                    response.Mensagem = "Plano não encontrado.";
                    return response;
                }

                var beneficiario = _mapper.Map<BeneficiarioModel>(beneficiarioCriacaoDto);

                await _context.AddAsync(beneficiario);
                await _context.SaveChangesAsync();

                response.Dados = _mapper.Map<BeneficiarioResponseDto>(beneficiario);
                response.Mensagem = "Beneficiário criado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao criar o beneficiário: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioResponseDto>> UpdateAsync(BeneficiarioEdicaoDto beneficiarioEdicaoDto)
        {
            var response = new ResponseModel<BeneficiarioResponseDto>();
            try
            {
                var beneficiarioBanco = await _context.Beneficiarios.FirstOrDefaultAsync(b => b.Id == beneficiarioEdicaoDto.Id);

                if (beneficiarioBanco == null)
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.NaoEncontrado;
                    response.Mensagem = "Beneficiário não localizado para edição.";
                    return response;
                }

                if (await _context.Beneficiarios.AnyAsync(b => b.Cpf == beneficiarioEdicaoDto.Cpf && b.Id != beneficiarioEdicaoDto.Id))
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.ErroValidacao;
                    response.Mensagem = "O CPF informado já está em uso por outro beneficiário.";
                    return response;
                }

                _mapper.Map(beneficiarioEdicaoDto, beneficiarioBanco);
                await _context.SaveChangesAsync();

                response.Dados = _mapper.Map<BeneficiarioResponseDto>(beneficiarioBanco);
                response.Mensagem = "Beneficiário editado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao editar o beneficiário: " + ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioModel>> DeleteAsync(int id)
        {
            var response = new ResponseModel<BeneficiarioModel>();
            try
            {
                var beneficiario = await _context.Beneficiarios.FirstOrDefaultAsync(b => b.Id == id);

                if (beneficiario == null)
                {
                    response.Status = false;
                    response.Tipo = TipoMensagem.NaoEncontrado;
                    response.Mensagem = "Beneficiário não localizado";
                    return response;
                }

                _context.Beneficiarios.Remove(beneficiario);
                await _context.SaveChangesAsync();

                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário removido com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Tipo = TipoMensagem.ErroServidor;
                response.Mensagem = "Ocorreu um erro ao deletar o beneficiário: " + ex.Message;
                return response;
            }
        }
    }
}