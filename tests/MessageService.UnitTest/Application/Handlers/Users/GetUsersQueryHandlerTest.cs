using Bogus;
using FluentAssertions;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.GetUsers.Queries;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Users
{
    public class GetUsersQueryHandlerTest
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
        public async Task Handle_Should_WhenUserDoesNot()
        {
            _mockUserRepository.Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(new List<User>());

            var handler = new GetUsersQueryHandler(_mockUserRepository.Object);
            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(0);
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError12);
        }

        [Test]
        public async Task Handle_Should_WhenResponseSuccess()
        {
            var faker = new Faker("tr");
            _mockUserRepository.Setup(x => x.GetAllAsync(null))
                .ReturnsAsync(new List<User>()
                {
                    User.Create(faker.Random.String(), faker.Random.String(), faker.Random.String())
                });

            var handler = new GetUsersQueryHandler(_mockUserRepository.Object);
            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);
            result.Success.Should().BeTrue();
            result.Result.Should().HaveCountGreaterThan(0);
        }
    }
}