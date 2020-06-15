using FluentAssertions;
using Xunit;

namespace Demo.Tests
{
    public class Demo0Tests
    {
        [Fact]
        public void Test1()
        {
            true.Should().BeTrue();
        }
    }
}