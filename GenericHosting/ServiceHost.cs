using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace GenericHosting
{
    public abstract class ServiceHost : IServiceHost
    {
        public ServiceHost(IHostApplicationLifetime application)
        {
            application.ApplicationStarted.Register(Started);
            application.ApplicationStopped.Register(Stopped);
            application.ApplicationStopping.Register(Stopping);
        }

        ~ServiceHost()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract Task StartAsync(CancellationToken cancellation);
        public abstract Task StopAsync(CancellationToken cancellation);

        protected virtual void Dispose(bool disposing) { }
        protected virtual void Started() { }
        protected virtual void Stopped() { }
        protected virtual void Stopping() { }
    }
}
