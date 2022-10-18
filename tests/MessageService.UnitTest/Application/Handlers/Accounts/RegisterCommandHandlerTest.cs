using Bogus;
using FluentAssertions;
using MessageService.Application.Constants;
using MessageService.Application.Features.Accounts.Register.Commands;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Accounts
{
    public class RegisterCommandHandlerTest
    {
        private Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [TearDown]
        public void Down()
        {
            _mockUserRepository.Reset();
        }

        [Test]
        public async Task Handle_Should_WhenRequestInValid()
        {
            var handler = new RegisterCommandHandler(_mockUserRepository.Object);
            var result = await handler.Handle(new RegisterCommand(), CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(1);
        }

        [Test]
        public async Task Handle_Should_WhenResultUserNameExists()
        {
            var faker = new Faker("tr");
            var userName = "ali.demir";
            var registerCommand = new RegisterCommand()
            {
                UserName = userName,
                FirstName = faker.Random.String(),
                LastName = faker.Random.String(),
                Password = faker.Random.String()
            };

            var user = User.Create(faker.Random.String(), faker.Random.String(), faker.Random.String());

            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == registerCommand.UserName))
                .ReturnsAsync(user);

            var handler = new RegisterCommandHandler(_mockUserRepository.Object);
            var result = await handler.Handle(registerCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterOrEqualTo(1);
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError6);
        }
        
        [Test]
        public async Task Handle_Should_WhenRegisterSuccess()
        {
            var faker = new Faker("tr");
            var registerCommand = new RegisterCommand()
            {
                UserName = faker.Person.UserName,
                Password = faker.Internet.Password(),
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName
            };
            var user = User.Create(registerCommand.FirstName,registerCommand.LastName,registerCommand.UserName);
            
            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == It.IsAny<string>()))
                .ReturnsAsync((User) null!);

            _mockUserRepository.Setup(x => x.AddAsync(user));

            var handler = new RegisterCommandHandler(_mockUserRepository.Object);
            var result = await handler.Handle(registerCommand, CancellationToken.None);

            result.Success.Should().BeTrue();
        }
    }
}