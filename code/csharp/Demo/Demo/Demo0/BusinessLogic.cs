using System;
using Polly;
using Polly.Timeout;
using Serilog;

namespace Demo.Demo0
{
    public class BusinessLogic
    {
        private readonly ISomeService _someService;
        private readonly ISyncPolicy _policy;

        public BusinessLogic(ISomeService someService, ISyncPolicy policy)
        {
            _someService = someService;
            _policy = policy;
            
            Log.Information("BusinessLogic ctor done");
        }

        public ServiceResult CallSomeSlowCode()
        {
            try
            {
                return _policy.Execute(() => _someService.SlowCode());
            }
            catch (TimeoutRejectedException ex)
            {
                Log.Information($"Ups: {ex.Message}");
                return ServiceResult.Timeout;
            }
        }

        public ServiceResult CallThrowingCode()
        {
            try
            {
                return _policy.Execute(() => _someService.ThrowingCode());
            }
            catch (MyException ex)
            {
                Log.Information($"Ups: {ex.Message}");
                return ServiceResult.Throw;
            }
        }
    }
}