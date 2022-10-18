using Bogus;
using FluentAssertions;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.GetUserLogs.Queries;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Users
{
    public class GetUserLogsQueryHandlerTest
    {
        private Mock<IUserLogRepository> _mockUserLogRepository;

        [SetUp]
        public void Setup()
        {
            _mockUserLogRepository = new Mock<IUserLogRepository>();
        }

        [TearDown]
        public void Down()
        {
            _mockUserLogRepository.Reset();
        }

        [Test]
        public async Task Handle_Should_WhenRequestInValid()
        {
            var handler = new GetUserLogsQueryHandler(_mockUserLogRepository.Object);
            var result = await handler.Handle(new GetUserLogsQuery(), CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task Handle_Should_WhenUserLogsDoesNot()
        {
            var faker = new Faker("tr");
            var getUserLogsQuery = new GetUserLogsQuery()
            {
                UserName = faker.Person.UserName
            };
            _mockUserLogRepository.Setup(x => x.GetAllAsync(x => x.UserName == getUserLogsQuery.UserName))
                .ReturnsAsync(new List<UserLog>());

            var handler = new GetUserLogsQueryHandler(_mockUserLogRepository.Object);
            var result = await handler.Handle(getUserLogsQuery, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(0);
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError10);
        }

        [Test]
        public async Task Handle_Should_WhenUserLogsSuccess()
        {
            var faker = new Faker("tr");
            var getUserLogsQuery = new GetUserLogsQuery()
            {
                UserName = faker.Person.UserName
            };
            _mockUserLogRepository.Setup(x => x.GetAllAsync(x => x.UserName == getUserLogsQuery.UserName))
                .ReturnsAsync(new List<UserLog>()
                {
                    UserLog.Create(faker.Random.String(), faker.Random.String())
                });

            var handler = new GetUserLogsQueryHandler(_mockUserLogRepository.Object);
            var result = await handler.Handle(getUserLogsQuery, CancellationToken.None);
            result.Success.Should().BeTrue();
            result.Result.Should().HaveCountGreaterThan(0);
        }
    }
}