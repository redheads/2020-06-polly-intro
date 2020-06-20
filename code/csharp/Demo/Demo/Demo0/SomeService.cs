using System;
using System.Threading;
using Serilog;

namespace Demo.Demo0
{
    public class SomeService : ISomeService
    {
        public ServiceResult SlowCode()
        {
            Thread.Sleep(1500);
            return ServiceResult.Success;
        }

        public ServiceResult ThrowingCode()
        {
            Log.Information("ThrowingCode called...");
            throw new Exception();
        }
    }
}