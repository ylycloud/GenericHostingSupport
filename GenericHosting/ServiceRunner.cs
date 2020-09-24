using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace GenericHosting
{
    public class ServiceRunner<TServiceHost>
        where TServiceHost : class, IServiceHost
    {
        private string[] args;

        protected virtual string EnvironmentVariablePrefix => null;

        public virtual async Task RunAsync(string[] args)
        {
            this.args = args;
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(this.ConfigureHostConfiguration)
                .ConfigureAppConfiguration(this.ConfigureAppConfiguration)
                .ConfigureServices(this.ConfigureServices)
                .ConfigureLogging(this.ConfigureLogging);

            await ConfigureAdditionalFeatures(hostBuilder).RunConsoleAsync();
        }

        protected virtual IHostBuilder ConfigureAdditionalFeatures(IHostBuilder hostBuilder)
            => hostBuilder;

        protected virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder config)
        {
            var dir = Directory.GetCurrentDirectory();
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: true);
            config.AddJsonFile(
                $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                optional: true);
            config.AddEnvironmentVariables(EnvironmentVariablePrefix);
            if (this.args != null)
            {
                config.AddCommandLine(this.args);
            }
        }

        protected virtual void ConfigureHostConfiguration(IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("hostsettings.json", true);
            config.AddEnvironmentVariables(EnvironmentVariablePrefix);
            if (this.args != null)
            {
                config.AddCommandLine(this.args);
            }
        }

        protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            logging.AddConsole();
        }

        protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IHostedService, TServiceHost>();
        }
    }
}
