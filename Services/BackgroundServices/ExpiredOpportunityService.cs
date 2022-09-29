using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.BackgroundServices
{
    public class ExpiredOpportunityService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        public ExpiredOpportunityService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            await base.StartAsync(stoppingToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var opportunityService = scope.ServiceProvider.GetRequiredService<Services.Opportunity.IOpportunityService>();
                    var personService = scope.ServiceProvider.GetRequiredService<Services.Person.IPersonService>();
                    var mailService = scope.ServiceProvider.GetRequiredService<Mail.MailService>();

                    foreach (var opportunityAtual in await opportunityService.ObterOpportunityExpired())
                    {
                        try
                        {
                            var opportunityViewModel = opportunityService.GetViewModelById(opportunityAtual);
                            var personViewModel = personService.GetViewModelById(opportunityViewModel.CompanyId);

                            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                            {
                                await opportunityService.UpdateStatusOpportunity(opportunityViewModel.OpportunityId.Value, DTO.Opportunity.StatusTypes.Expirada);
                                transactionScope.Complete();
                            }
                            string emailCompany = personViewModel.Email;
                            await mailService.SendAsync(Mail.Templates.Assunto(opportunityViewModel), Mail.Templates.OpportunityExpired(personViewModel.CompanyName, opportunityViewModel), new List<string>() { emailCompany}, null, true);
                        }
                        catch (Exception exception)
                        {
                            throw exception;
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromHours(8), stoppingToken);
            }
        }
        
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
