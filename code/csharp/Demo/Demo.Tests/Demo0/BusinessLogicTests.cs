using System;
using Demo.Demo0;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Polly;
using Polly.Timeout;
using Xunit;

namespace Demo.Tests.Demo0
{
    public class BusinessLogicTests
    {
        [Fact]
        public void Returns_ServiceResult_Timeout_when_exception_is_thrown()
        {
            // Arrange
            var someService = Substitute.For<ISomeService>();
            someService.SlowCode()
                .Returns(ServiceResult.Success);

            var syncPolicy = Substitute.For<ISyncPolicy>();
            syncPolicy
                .Execute(Arg.Any<Func<ServiceResult>>())
                .Throws(new TimeoutRejectedException());

            var sut = new BusinessLogic(someService, syncPolicy);
            
            // Act
            var result = sut.CallSomeSlowCode();
            
            // Assert
            result.Should().Be(ServiceResult.Timeout);
        }

        [Fact]
        public void Returns_ServiceResult_Throw()
        {
            // Arrange
            var someService = Substitute.For<ISomeService>();
            someService.ThrowingCode()
                .Throws(new MyException());

            var syncPolicy = Substitute.For<ISyncPolicy>();
            syncPolicy
                .Execute(Arg.Any<Func<ServiceResult>>())
                .Throws(new MyException());

            var sut = new BusinessLogic(someService, syncPolicy);
            
            // Act
            var result = sut.CallThrowingCode();
            
            // Assert
            result.Should().Be(ServiceResult.Throw);
        }
    }
}