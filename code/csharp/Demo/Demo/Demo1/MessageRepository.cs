using System;
using Serilog;

namespace Demo.Demo1
{
    public interface IMessageRepository
    {
        MessageResult GetHelloMessage();
        MessageResult GetGoodbyeMessage();
    }
    
    public class MessageRepository : IMessageRepository
    {
        public MessageResult GetHelloMessage()
        {
            Log.Information("MessageRepository GetHelloMessage running");
            ThrowRandomException();
            return MessageResult.Hello;
        }

        public MessageResult GetGoodbyeMessage()
        {
            Log.Information("MessageRepository GetGoodbyeMessage running");
            ThrowRandomException();
            return MessageResult.Goodbye;
        }
        
        private void ThrowRandomException()
        {
            var diceRoll = new Random().Next(0, 10);

            if (diceRoll > 5)
            {
                Log.Information("ERROR! Throwing Exception");
                throw new Exception("Exception in MessageRepository");
            }
        }    
    }
}