using Demo.Demo0;
using Polly;
using Polly.Timeout;
using Serilog;

namespace Demo.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            InitLogger();

            Demo0();
        }

        private static void Demo0()
        {
            var timeoutPolicy = Policy.Timeout(1, TimeoutStrategy.Pessimistic);

            var businessLogic = new BusinessLogic(new SomeService(), timeoutPolicy);

            var result = businessLogic.CallSomeSlowCode();

            Log.Information($"Result: {result}");
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}