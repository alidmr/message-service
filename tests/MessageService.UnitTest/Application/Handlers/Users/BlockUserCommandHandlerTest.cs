using Bogus;
using FluentAssertions;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.BlockUsers.Command;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Users
{
    public class BlockUserCommandHandlerTest
    {
        private Mock<IBlockUserRepository> _mockBlockUserRepository;
        private Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockBlockUserRepository = new Mock<IBlockUserRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [TearDown]
        public void Down()
        {
            _mockBlockUserRepository.Reset();
            _mockUserRepository.Reset();
        }

        [Test]
        public async Task Handle_Should_WhenRequestInValid()
        {
            var handler = new BlockUserCommandHandler(_mockBlockUserRepository.Object, _mockUserRepository.Object);
            var result = await handler.Handle(new BlockUserCommand(), CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(1);
        }

        [Test]
        public async Task Handle_Should_WhenBlockedUserNull()
        {
            var faker = new Faker("tr");
            var blockedUserCommand = new BlockUserCommand()
            {
                BlockingUserName = faker.Person.UserName,
                BlockedUserName = faker.Person.UserName
            };
            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == blockedUserCommand.BlockedUserName))
                .ReturnsAsync((User) null!);

            var handler = new BlockUserCommandHandler(_mockBlockUserRepository.Object, _mockUserRepository.Object);
            var result = await handler.Handle(blockedUserCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError9);
        }

        [Test]
        public async Task Handle_Should_WhenResponseSuccess()
        {
            var faker = new Faker("tr");
            var blockedUserCommand = new BlockUserCommand()
            {
                BlockingUserName = faker.Person.UserName,
                BlockedUserName = faker.Person.UserName
            };
            var checkUser = User.Create(faker.Random.String(), faker.Random.String(), faker.Random.String());
            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == blockedUserCommand.BlockedUserName))
                .ReturnsAsync(checkUser);

            var blockUser = BlockUser.Create(blockedUserCommand.BlockingUserName, blockedUserCommand.BlockedUserName);
            _mockBlockUserRepository.Setup(x => x.AddAsync(blockUser));

            var handler = new BlockUserCommandHandler(_mockBlockUserRepository.Object, _mockUserRepository.Object);
            var result = await handler.Handle(blockedUserCommand, CancellationToken.None);
            result.Success.Should().BeTrue();
        }
    }
}