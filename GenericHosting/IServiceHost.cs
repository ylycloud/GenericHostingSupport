using Microsoft.Extensions.Hosting;
using System;

namespace GenericHosting
{
    public interface IServiceHost : IHostedService, IDisposable
    {
    }
}
