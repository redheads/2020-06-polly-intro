using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Demo.Tests
{
    public class Demo0Tests
    {
        [Fact]
        public void IsAdult_happy_case()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            userService.GetAge(Arg.Any<int>()).Returns(42);
            
            // Act
            var sut = new Demo0(userService);

            // Assert
            sut.IsAdult(1).Should().BeTrue();
        }
    }
}