using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoAppApi.Services.KeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAppApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        var config = configBuilder.Build();
                        KeyVaultOptions kvOptions = new();
                        config.GetSection(KeyVaultOptions.Position).Bind(kvOptions);

                        var credential = new ClientSecretCredential(kvOptions.TenantId, kvOptions.ClientId, kvOptions.ClientSecret);

                        var client = new SecretClient(new Uri(kvOptions.KVUrl), credential);
                        configBuilder.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());
                    }
                    else
                    {
                        string vaultUri = Environment.GetEnvironmentVariable("VaultUri");
                        var keyVaultEndpoint = new Uri(vaultUri);
                        configBuilder.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
