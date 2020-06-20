using System;
using Demo.Demo0;
using Polly;
using Polly.Timeout;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Demo.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            InitLogger();

            // Demo0();
            // Demo1();
            Demo2();
        }

        private static void Demo0()
        {
            var timeoutPolicy = Policy.Timeout(1, TimeoutStrategy.Pessimistic);

            var someService = new SomeService();
            
            var businessLogic = new BusinessLogic(someService, timeoutPolicy);

            var result = businessLogic.CallSomeSlowCode();

            Log.Information($"Result: {result}");
        }

        private static void Demo1()
        {
            var policy = Policy
                .Handle<MyException>()
                .WaitAndRetry(3, x => TimeSpan.FromSeconds(2));

            var someService = new SomeService();
            
            var businessLogic = new BusinessLogic(someService, policy);

            var result = businessLogic.CallThrowingCode();

            Log.Information($"Result: {result}");
        }

        private static void Demo2()
        {
            var policy = Policy
                .Handle<MyException>()
                .WaitAndRetry(new []
                {
                    TimeSpan.FromSeconds(1), 
                    TimeSpan.FromSeconds(2), 
                    TimeSpan.FromSeconds(4), 
                    TimeSpan.FromSeconds(8) 
                });

            var someService = new SomeService();
            
            var businessLogic = new BusinessLogic(someService, policy);

            var result = businessLogic.CallThrowingCode();

            Log.Information($"Result: {result}");
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: ConsoleTheme.None)
                .CreateLogger();
        }
    }
}