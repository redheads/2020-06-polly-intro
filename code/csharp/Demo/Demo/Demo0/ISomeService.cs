namespace Demo.Demo0
{
    /// <summary>
    /// We know that `SomeService` is not very reliable...
    /// </summary>
    public interface ISomeService
    {
        // method is called synchronously...
        ServiceResult SlowCode();
        ServiceResult ThrowingCode();
    }
}