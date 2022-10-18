using Bogus;
using FluentAssertions;
using MessageService.Domain.Constants;
using MessageService.Domain.Entities;
using MessageService.Domain.Exceptions;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Domain
{
    public class UserTest
    {
        [Test]
        public void Create_ShouldException_WhenFirstNameNull()
        {
            var result = Assert.Throws<DomainException>(() => { User.Create(string.Empty, It.IsAny<string>(), It.IsAny<string>()); });
            result.Should().NotBeNull();
            result?.Message.Should().Be(DomainErrorMessage.DomainError1);
        }

        [Test]
        public void Create_ShouldException_WhenLastNameNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() => { User.Create(faker.Person.FirstName, string.Empty, faker.Person.UserName); });
            result.Should().NotBeNull();
            result?.Message.Should().Be(DomainErrorMessage.DomainError2);
        }

        [Test]
        public void Create_ShouldException_WhenUserNameNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() => { User.Create(faker.Person.FirstName, faker.Person.LastName, string.Empty); });
            result.Should().NotBeNull();
            result?.Message.Should().Be(DomainErrorMessage.DomainError4);
        }

        [Test]
        public void Create_Should_ResponseSuccess()
        {
            var faker = new Faker("tr");
            var result = User.Create(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName);
            result.Should().NotBeNull();
        }

        [Test]
        public void SetPassword_Should_WhenPasswordNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() =>
            {
                var user = User.Create(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName);
                user.SetPassword(string.Empty, It.IsAny<string>());
            });
            result.Should().NotBeNull();
            result.Message.Should().Be(DomainErrorMessage.DomainError5);
        }

        [Test]
        public void SetPassword_Should_WhenPasswordSaltNull()
        {
            var faker = new Faker("tr");
            var result = Assert.Throws<DomainException>(() =>
            {
                var user = User.Create(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName);
                user.SetPassword(faker.Internet.Password(), string.Empty);
            });
            result.Should().NotBeNull();
            result.Message.Should().Be(DomainErrorMessage.DomainError6);
        }

        [Test]
        public void SetPassword_Should_ResponseSuccess()
        {
            var faker = new Faker("tr");
            var user = User.Create(faker.Person.FirstName, faker.Person.LastName, faker.Person.UserName);
            user.SetPassword(faker.Internet.Password(), faker.Random.String());

            user.Should().NotBeNull();
            user.Password.Should().NotBeNull();
            user.PasswordSalt.Should().NotBeNull();
        }
    }
}