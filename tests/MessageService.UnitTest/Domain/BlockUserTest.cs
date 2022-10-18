using Bogus;
using FluentAssertions;
using MessageService.Domain.Constants;
using MessageService.Domain.Entities;
using MessageService.Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Domain
{
    public class BlockUserTest
    {
        [Test]
        public void Create_Should_WhenBlockingUserNull()
        {
            var result = Assert.Throws<DomainException>(() => { BlockUser.Create(string.Empty, It.IsAny<string>()); });
            result.Message.Should().Be(DomainErrorMessage.DomainError14);
        }

        [Test]
        public void Create_Should_WhenBlockedUserNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() => { BlockUser.Create(faker.Random.String(), string.Empty); });
            result.Message.Should().Be(DomainErrorMessage.DomainError15);
        }

        [Test]
        public void Create_Should_ResponseSuccess()
        {
            var faker = new Faker("tr");
            var result = BlockUser.Create(faker.Random.String(), faker.Random.String());
            result.Should().NotBeNull();
        }
    }
}