using Bogus;
using FluentAssertions;
using MessageService.Application.Constants;
using MessageService.Application.Features.Accounts.Login.Commands;
using MessageService.Application.Services.Token;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using MessageService.Infrastructure.Dtos;
using MessageService.Infrastructure.Helpers;
using MessageService.Infrastructure.Services.RabbitMq;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Accounts
{
    public class LoginCommandHandlerTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ITokenService> _mockTokenService;
        private Mock<IRabbitMqService> _mockRabbitMqService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _mockRabbitMqService = new Mock<IRabbitMqService>();
        }

        [TearDown]
        public void Down()
        {
            _mockUserRepository.Reset();
            _mockTokenService.Reset();
            _mockRabbitMqService.Reset();
        }

        [Test]
        public async Task Handle_Should_WhenRequestInValid()
        {
            var handler = new LoginCommandHandler(_mockUserRepository.Object, _mockTokenService.Object, _mockRabbitMqService.Object);
            var result = await handler.Handle(new LoginCommand(), CancellationToken.None);
            result.Success.Should().BeFalse();
        }

        [Test]
        public async Task Handle_Should_WhenUserNull()
        {
            _mockUserRepository.Setup(x => x.GetAsync(user => user.UserName == It.IsAny<string>()))
                .ReturnsAsync((User) null!);

            var faker = new Faker("tr");
            var loginCommand = new LoginCommand()
            {
                UserName = faker.Random.String(), Password = faker.Random.String()
            };

            var handler = new LoginCommandHandler(_mockUserRepository.Object, _mockTokenService.Object, _mockRabbitMqService.Object);
            var result = await handler.Handle(loginCommand, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError8);
        }

        [Test]
        public async Task Handle_Should_WhenPasswordWrong()
        {
            var faker = new Faker("tr");
            var user = User.Create(faker.Random.String(), faker.Random.String(), faker.Random.String());
            var loginCommand = new LoginCommand()
            {
                UserName = faker.Random.String(),
                Password = faker.Random.String()
            };
            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == loginCommand.UserName))
                .ReturnsAsync(user);

            var handler = new LoginCommandHandler(_mockUserRepository.Object, _mockTokenService.Object, _mockRabbitMqService.Object);

            var result = await handler.Handle(loginCommand, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError8);
        }

        [Test]
        public async Task Handle_Should_WhenLoginSuccess()
        {
            var faker = new Faker("tr");
            var salt = HashingHelper.GetSalt();
            var password = HashingHelper.HashPassword("123qwe", salt);

            var user = User.Create(faker.Random.String(), faker.Random.String(), faker.Random.String());
            user.SetPassword(password, salt);
            var token = new TokenDto()
            {
                Token = "test token",
                ExpirationDate = DateTime.Now.AddMinutes(5)
            };
            var loginCommand = new LoginCommand()
            {
                UserName = "ali.demir",
                Password = "123qwe"
            };

            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == loginCommand.UserName))
                .ReturnsAsync(user);

            _mockTokenService.Setup(x => x.CreateToken(user)).Returns(token);

            var handler = new LoginCommandHandler(_mockUserRepository.Object, _mockTokenService.Object, _mockRabbitMqService.Object);

            var result = await handler.Handle(loginCommand, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.Token.Should().Be(token);
        }
    }
}