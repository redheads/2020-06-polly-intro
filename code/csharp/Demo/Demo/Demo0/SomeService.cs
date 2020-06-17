using System.Threading;

namespace Demo.Demo0
{
    public class SomeService : ISomeService
    {
        public ServiceResult SlowCode()
        {
            Thread.Sleep(1500);
            return ServiceResult.Success;
        }
    }
}