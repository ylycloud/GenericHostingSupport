using System.Threading.Tasks;

namespace GenericHosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceRunner = new ServiceRunner<RunService>();
            await serviceRunner.RunAsync(args);
        }
    }
}
