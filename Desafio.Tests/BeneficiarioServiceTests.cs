using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Beneficiario;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Services;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Enums;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Desafio_Tecnico_Tests.Services
{
    public class BeneficiarioServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BeneficiarioService _service;

        public BeneficiarioServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _mapperMock = new Mock<IMapper>();

            _service = new BeneficiarioService(_context, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_DeveRetornarSucesso_QuandoDadosValidos()
        {
            // Arrange
            var planoId = 1;
         
            await _context.Planos.AddAsync(new PlanoModel { Id = planoId, Nome = "Plano Teste", Codigo_registro_ans = "123" });
            await _context.SaveChangesAsync();

            var dtoCriacao = new BeneficiarioCriacaoDto { NomeCompleto = "Teste", Cpf = "12345678900", PlanoId = planoId };
            var modelMapeado = new BeneficiarioModel { Id = 1, NomeCompleto = "Teste", Cpf = "12345678900", PlanoId = planoId };
            var dtoRetorno = new BeneficiarioResponseDto { Id = 1, NomeCompleto = "Teste" };

            _mapperMock.Setup(m => m.Map<BeneficiarioModel>(dtoCriacao)).Returns(modelMapeado);
            _mapperMock.Setup(m => m.Map<BeneficiarioResponseDto>(modelMapeado)).Returns(dtoRetorno);

            // Act
            var resultado = await _service.CreateAsync(dtoCriacao);

            // Assert
            Assert.True(resultado.Status);
            Assert.Equal("Beneficiário criado com sucesso.", resultado.Mensagem);
            Assert.NotNull(resultado.Dados);
        }

        [Fact]
        public async Task CreateAsync_DeveRetornarErro_QuandoCpfJaExiste()
        {
            // Arrange
            var cpfExistente = "11122233344";
            await _context.Beneficiarios.AddAsync(new BeneficiarioModel { NomeCompleto = "Existente", Cpf = cpfExistente });
            await _context.SaveChangesAsync();

            var dtoCriacao = new BeneficiarioCriacaoDto { NomeCompleto = "Novo", Cpf = cpfExistente, PlanoId = 1 };

            // Act
            var resultado = await _service.CreateAsync(dtoCriacao);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.ErroValidacao, resultado.Tipo);
            Assert.Equal("CPF já cadastrado para outro beneficiário.", resultado.Mensagem);
        }

        [Fact]
        public async Task CreateAsync_DeveRetornarErro_QuandoPlanoNaoExiste()
        {
            // Arrange
            var dtoCriacao = new BeneficiarioCriacaoDto { NomeCompleto = "Novo", Cpf = "99988877700", PlanoId = 99 };

            // Act
            var resultado = await _service.CreateAsync(dtoCriacao);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.NaoEncontrado, resultado.Tipo);
            Assert.Equal("Plano não encontrado.", resultado.Mensagem);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarBeneficiario_QuandoIdExiste()
        {
            // Arrange
            var idExistente = 10;
            var beneficiario = new BeneficiarioModel { Id = idExistente, NomeCompleto = "Teste Busca", Cpf = "00000000000" };
            await _context.Beneficiarios.AddAsync(beneficiario);
            await _context.SaveChangesAsync();

            _mapperMock.Setup(m => m.Map<BeneficiarioResponseDto>(It.IsAny<BeneficiarioModel>())) // Correto: Aceita qualquer BeneficiarioModel
                       .Returns(new BeneficiarioResponseDto { Id = idExistente, NomeCompleto = "Teste Busca" });

            // Act
            var resultado = await _service.GetByIdAsync(idExistente);

            // Assert
            Assert.True(resultado.Status);
            Assert.NotNull(resultado.Dados);
            Assert.Equal(idExistente, resultado.Dados.Id);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNaoEncontrado_QuandoIdInexistente()
        {
            // Arrange
            var idInexistente = 999;

            // Act
            var resultado = await _service.GetByIdAsync(idInexistente);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.NaoEncontrado, resultado.Tipo);
        }

        [Fact]
        public async Task UpdateAsync_DeveRetornarErro_QuandoCpfPertenceAOutro()
        {
            // Arrange
            var cpfUsuarioB = "22222222222";

            await _context.Beneficiarios.AddAsync(new BeneficiarioModel { Id = 1, NomeCompleto = "User A", Cpf = "11111111111" });
            await _context.Beneficiarios.AddAsync(new BeneficiarioModel { Id = 2, NomeCompleto = "User B", Cpf = cpfUsuarioB });
            await _context.SaveChangesAsync();

            var dtoEdicao = new BeneficiarioEdicaoDto { Id = 1, NomeCompleto = "User A Editado", Cpf = cpfUsuarioB };

            // Act
            var resultado = await _service.UpdateAsync(dtoEdicao);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.ErroValidacao, resultado.Tipo);
            Assert.Contains("CPF informado já está em uso", resultado.Mensagem);
        }

        [Fact]
        public async Task DeleteAsync_DeveAgendarExclusao_QuandoIdExiste()
        {
            // Arrange
            var idParaRemover = 5;
            await _context.Beneficiarios.AddAsync(new BeneficiarioModel
            {
                Id = idParaRemover,
                NomeCompleto = "Para Remover",
                Cpf = "555",
                PendenteExclusao = false 
            });
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _service.DeleteAsync(idParaRemover, 3);

            var buscaAposRemocao = await _context.Beneficiarios.FindAsync(idParaRemover);

            // Assert
            Assert.True(resultado.Status);

            
            Assert.NotNull(buscaAposRemocao);
            Assert.True(buscaAposRemocao.PendenteExclusao);
        }
    }
}