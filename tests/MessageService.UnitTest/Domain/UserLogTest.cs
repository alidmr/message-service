using Bogus;
using FluentAssertions;
using MessageService.Domain.Constants;
using MessageService.Domain.Entities;
using MessageService.Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Domain
{
    public class UserLogTest
    {
        [Test]
        public void Create_Should_WhenUserNameNull()
        {
            var result = Assert.Throws<DomainException>(() =>
            {
                UserLog.Create(string.Empty, It.IsAny<string>());
            });
            result.Message.Should().Be(DomainErrorMessage.DomainError7);
        }
        [Test]
        public void Create_Should_WhenContentNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() =>
            {
                UserLog.Create(faker.Random.String(), string.Empty);
            });
            result.Message.Should().Be(DomainErrorMessage.DomainError8);
        }
        [Test]
        public void Create_Should_ResponseSuccess()
        {
            var faker = new Faker("tr");
            var result = UserLog.Create(faker.Random.String(), faker.Random.String());
            result.Should().NotBeNull();
        }
    }
}