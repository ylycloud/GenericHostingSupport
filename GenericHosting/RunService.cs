using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace GenericHosting
{
    public class RunService : ServiceHost
    {
        private readonly ILogger _logger;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private readonly List<Task> tasks = new List<Task>();

        public RunService(ILogger<RunService> logger, IHostApplicationLifetime applicationLifetime)
            : base(applicationLifetime)
        {
            this._logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var task = Task.Run(async () =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    _logger.LogInformation($"Task executing at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                    await Task.Delay(1000);
                }
            });
            tasks.Add(task);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Task.WaitAll(tasks.ToArray(), 5000);
            _logger.LogInformation("Host stopped.");
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            _logger.LogInformation("Host disposed.");
            base.Dispose(disposing);
        }

        protected override void Stopped()
        {
            _logger.LogInformation("Host stopping requested.");
            this.cancellation.Cancel();
        }
    }
}
