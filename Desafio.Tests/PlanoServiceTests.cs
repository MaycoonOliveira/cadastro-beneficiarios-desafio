using AutoMapper;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Dto.Plano;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Application.Services;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Entities;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Domain.Enums;
using Desafio_Tecnico_Cadastro_de_Beneficiarios.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Desafio_Tecnico_Tests.Services
{
    public class PlanoServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PlanoService _service;

        public PlanoServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _mapperMock = new Mock<IMapper>();
            _service = new PlanoService(_context, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_DeveCriarPlano_QuandoDadosValidos()
        {
            // Arrange
            var dtoCriacao = new PlanoCriacaoDto { Nome = "Plano Ouro", Codigo_registro_ans = "123456" };
            var modelMapeado = new PlanoModel { Id = 1, Nome = "Plano Ouro", Codigo_registro_ans = "123456" };

            _mapperMock.Setup(m => m.Map<PlanoModel>(It.IsAny<PlanoCriacaoDto>())).Returns(modelMapeado);
            _mapperMock.Setup(m => m.Map<PlanoResponseDto>(It.IsAny<PlanoModel>()))
                       .Returns(new PlanoResponseDto { Id = 1, Nome = "Plano Ouro" });

            // Act
            var resultado = await _service.CreateAsync(dtoCriacao);

            // Assert
            Assert.True(resultado.Status);
            Assert.Equal("Plano criado com sucesso", resultado.Mensagem);
            Assert.Equal(1, await _context.Planos.CountAsync());
        }

        [Fact]
        public async Task CreateAsync_DeveRetornarErro_QuandoCodigoANSJaExiste()
        {
            // Arrange
            var ansDuplicado = "999999";

            await _context.Planos.AddAsync(new PlanoModel { Nome = "Existente", Codigo_registro_ans = ansDuplicado });
            await _context.SaveChangesAsync();

            var dtoCriacao = new PlanoCriacaoDto { Nome = "Novo", Codigo_registro_ans = ansDuplicado };

            // Act
            var resultado = await _service.CreateAsync(dtoCriacao);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.ErroValidacao, resultado.Tipo);
            Assert.Contains("já existe", resultado.Mensagem);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizar_QuandoIdExiste()
        {
            // Arrange
            var idPlano = 5;
            await _context.Planos.AddAsync(new PlanoModel { Id = idPlano, Nome = "Antigo", Codigo_registro_ans = "555" });
            await _context.SaveChangesAsync();

            var dtoEdicao = new PlanoEdicaoDto { Id = idPlano, Nome = "Novo Nome", Codigo_registro_ans = "555" };

            _mapperMock.Setup(m => m.Map(It.IsAny<PlanoEdicaoDto>(), It.IsAny<PlanoModel>()));
            _mapperMock.Setup(m => m.Map<PlanoResponseDto>(It.IsAny<PlanoModel>()))
                       .Returns(new PlanoResponseDto { Id = idPlano, Nome = "Novo Nome" });

            // Act
            var resultado = await _service.UpdateAsync(dtoEdicao);

            // Assert
            Assert.True(resultado.Status);
            Assert.Equal("Plano editado com sucesso", resultado.Mensagem);
        }

        [Fact]
        public async Task UpdateAsync_DeveRetornarErro_QuandoTentaUsarANSDeOutroPlano()
        {
            // Arrange
            await _context.Planos.AddAsync(new PlanoModel { Id = 1, Nome = "Plano A", Codigo_registro_ans = "111" });
            await _context.Planos.AddAsync(new PlanoModel { Id = 2, Nome = "Plano B", Codigo_registro_ans = "222" });
            await _context.SaveChangesAsync();

            var dtoEdicao = new PlanoEdicaoDto { Id = 1, Nome = "Plano A Editado", Codigo_registro_ans = "222" };

            // Act
            var resultado = await _service.UpdateAsync(dtoEdicao);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.ErroValidacao, resultado.Tipo);
            Assert.Contains("já está em uso por outro plano", resultado.Mensagem);
        }

        [Fact]
        public async Task DeletarPlano_DeveRetornarSucesso_QuandoNaoTemBeneficiarios()
        {
            // Arrange
            var idParaRemover = 10;
            await _context.Planos.AddAsync(new PlanoModel { Id = idParaRemover, Nome = "Vazio", Codigo_registro_ans = "000" });
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _service.DeletarPlano(idParaRemover);

            // Assert
            Assert.True(resultado.Status);
            Assert.Equal("Plano removido com sucesso", resultado.Mensagem);
            Assert.Null(await _context.Planos.FindAsync(idParaRemover));
        }

        [Fact]
        public async Task DeletarPlano_DeveBloquearExclusao_QuandoTemBeneficiarios()
        {
            // Arrange
            var idPlanoComFilhos = 20;
            var plano = new PlanoModel { Id = idPlanoComFilhos, Nome = "Com Beneficiarios", Codigo_registro_ans = "XXX" };

            await _context.Planos.AddAsync(plano);
            await _context.Beneficiarios.AddAsync(new BeneficiarioModel { NomeCompleto = "Cliente", Cpf = "123", PlanoId = idPlanoComFilhos });
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _service.DeletarPlano(idPlanoComFilhos);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.ErroValidacao, resultado.Tipo);
            Assert.Equal("Não é possível deletar um plano que possui beneficiários associados.", resultado.Mensagem);

            Assert.NotNull(await _context.Planos.FindAsync(idPlanoComFilhos));
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNaoEncontrado_QuandoIdInexistente()
        {
            // Arrange

            // Act
            var resultado = await _service.GetByIdAsync(999);

            // Assert
            Assert.False(resultado.Status);
            Assert.Equal(TipoMensagem.NaoEncontrado, resultado.Tipo);
        }
    }
}