using System;
using System.Threading;
using Demo.Demo0;
using Demo.Demo1;
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

            // Demo0a();
            // Demo0b();
            // Demo0c();
            
            Demo1a();
        }

        private static void Demo0a()
        {
            var policy = Policy
                .Timeout(1, TimeoutStrategy.Pessimistic);

            var someService = new SomeService();
            
            var businessLogic = new BusinessLogic(someService, policy);

            var result = businessLogic.CallSomeSlowCode();

            Log.Information($"Result: {result}");
        }

        private static void Demo0b()
        {
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(3, x => TimeSpan.FromSeconds(2));

            var someService = new SomeService();
            
            var businessLogic = new BusinessLogic(someService, policy);

            var result = businessLogic.CallThrowingCode();

            Log.Information($"Result: {result}");
        }

        private static void Demo0c()
        {
            var policy = Policy
                .Handle<Exception>()
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

        private static void Demo1a()
        {
            var messageService = new MessageService(new MessageRepository());
            for (var i = 1; i <= 10; i++)
            {
                Log.Information($"Loop {i} started. Waiting 1sec...");
                Thread.Sleep(1000);
                messageService.GetGoodbyeMessage();
            }
        }
        
        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: ConsoleTheme.None)
                .CreateLogger();
        }
    }
}