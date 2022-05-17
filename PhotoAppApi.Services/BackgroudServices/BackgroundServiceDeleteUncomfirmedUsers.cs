using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhotoAppApi.DAL.Generic;
using PhotoAppApi.EF;
using PhotoAppApi.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoAppApi.Services.BackgroudServices
{
    public class BackgroundServiceDeleteUncomfirmedUsers : BackgroundService 
    {
        private static TimeSpan _period =
            new TimeSpan(0, 1, 0, 0); 

        private readonly IServiceScopeFactory _scopeFactory; 

        public BackgroundServiceDeleteUncomfirmedUsers(
            IServiceScopeFactory scopeFactory,
            TimeSpan periodOverride = default)
        {
            _scopeFactory = scopeFactory;

            if (periodOverride != default)
                _period = periodOverride;
        }

        protected override async Task ExecuteAsync    
            (CancellationToken stoppingToken)   
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                await DoWorkAsync();
                await Task.Delay(_period, stoppingToken);
            }
        }
        private async Task DoWorkAsync() 
        {
            using (var scope = _scopeFactory.CreateScope()) 
            {
                var now = DateTime.UtcNow;
                var usersRepo = scope.ServiceProvider       
                    .GetRequiredService<IGenericRepository<User>>();
                await usersRepo.RemoveAsync(u => u.Confirmed == false && u.RegistrationDate < now.Subtract(_period));
            }
        }
    }
}
