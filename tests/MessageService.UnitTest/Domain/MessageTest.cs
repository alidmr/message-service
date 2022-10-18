using Bogus;
using FluentAssertions;
using MessageService.Domain.Constants;
using MessageService.Domain.Entities;
using MessageService.Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Domain
{
    public class MessageTest
    {
        [Test]
        public void Create_Should_WhenSenderNull()
        {
            var result = Assert.Throws<DomainException>(() => { Message.Create(string.Empty, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()); });
            result.Message.Should().Be(DomainErrorMessage.DomainError9);
        }

        [Test]
        public void Create_Should_WhenSenderUserNameNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() => { Message.Create(faker.Person.FirstName, string.Empty, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()); });
            result.Message.Should().Be(DomainErrorMessage.DomainError12);
        }

        [Test]
        public void Create_Should_WhenReceiverNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() => { Message.Create(faker.Person.FirstName, faker.Person.FirstName, string.Empty, It.IsAny<string>(), It.IsAny<string>()); });
            result.Message.Should().Be(DomainErrorMessage.DomainError10);
        }

        [Test]
        public void Create_Should_WhenReceiverUserNameNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() => { Message.Create(faker.Person.FirstName, faker.Person.FirstName, faker.Person.FirstName, string.Empty, It.IsAny<string>()); });
            result.Message.Should().Be(DomainErrorMessage.DomainError13);
        }

        [Test]
        public void Create_Should_WhenContentNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() => { Message.Create(faker.Person.FirstName, faker.Person.FirstName, faker.Person.FirstName, faker.Person.FirstName, string.Empty); });
            result.Message.Should().Be(DomainErrorMessage.DomainError11);
        }

        [Test]
        public void Create_Should_ResponseSuccess()
        {
            var faker = new Faker("tr");
            var result = Message.Create(faker.Person.FirstName, faker.Person.FirstName, faker.Person.FirstName, faker.Person.FirstName, faker.Random.String());
            result.Should().NotBeNull();
        }
    }
}