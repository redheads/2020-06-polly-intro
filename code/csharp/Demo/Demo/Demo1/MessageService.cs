using System;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Serilog;

namespace Demo.Demo1
{
    public interface IMessageService
    {
        MessageResult GetHelloMessage();
        MessageResult GetGoodbyeMessage();
    }

    // Idea from: https://medium.com/@therealjordanlee/retry-circuit-breaker-patterns-in-c-with-polly-9aa24c5fe23a
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        private readonly RetryPolicy _retryPolicy;
        private readonly CircuitBreakerPolicy _circuitBreakerPolicy;
        
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(2, retryAttempt =>
                {
                    var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    Log.Information($"Waiting {timeToWait.TotalSeconds} seconds");
                    return timeToWait;
                });
            
            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromSeconds(1),
                    (ex, t) =>
                    {
                        Log.Information("Circuit broken!");
                    },
                    () =>
                    {
                        Log.Information("Circuit Reset!");
                    });
            
        }
        
        public MessageResult GetHelloMessage()
        {
            try
            {
                return _retryPolicy.Execute(() => _messageRepository.GetHelloMessage());
            }
            catch (Exception ex)
            {
                Log.Information(ex.Message);
                return MessageResult.Exception;
            }
        }
        
        public MessageResult GetGoodbyeMessage()
        {
            try
            {
                Log.Information($"Circuit State: {_circuitBreakerPolicy.CircuitState}");
                return _circuitBreakerPolicy.Execute(() => _messageRepository.GetGoodbyeMessage());
            }
            catch (Exception ex)
            {
                Log.Information(ex.Message);
                return MessageResult.Exception;
            }
        }
    }
}