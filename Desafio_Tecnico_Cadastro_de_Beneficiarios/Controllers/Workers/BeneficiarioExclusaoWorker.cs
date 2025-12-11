using Desafio_Tecnico_Cadastro_de_Beneficiarios.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Workers
{
    public class BeneficiarioExclusaoWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BeneficiarioExclusaoWorker> _logger;

        public BeneficiarioExclusaoWorker(IServiceProvider serviceProvider, ILogger<BeneficiarioExclusaoWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("-----------Worker de Exclusão INICIADO-----------.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"-----------[{DateTime.Now:HH:mm:ss}] Verificando fila de exclusão...-----------");

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var fila = await context.Beneficiarios
                            .Where(b => b.PendenteExclusao == true)
                            .OrderBy(b => b.PrioridadeExclusao) 
                            .ThenBy(b => b.DataSolicitacaoExclusao)
                            .ToListAsync(stoppingToken);

                        if (fila.Any())
                        {
                            _logger.LogWarning($"-----------Encontrados {fila.Count} itens para excluir-----------.");

                            foreach (var item in fila)
                            {
                                context.Beneficiarios.Remove(item);
                                _logger.LogInformation($"-----------Excluindo ID: {item.Id} | Nome: {item.NomeCompleto} | Prioridade: {item.PrioridadeExclusao}-----------");
                            }

                            await context.SaveChangesAsync(stoppingToken);
                            _logger.LogInformation("-----------Fila processada com sucesso-----------.");
                        }
                        else
                        {
                            _logger.LogInformation("-----------Nenhum item pendente-----------.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro no Worker: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}