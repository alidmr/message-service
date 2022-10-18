using Bogus;
using FluentAssertions;
using MessageService.Application.Constants;
using MessageService.Application.Features.Users.GetUserMessages.Queries;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Users
{
    public class GetUserMessagesQueryHandlerTest
    {
        private Mock<IMessageRepository> _mockMessageRepository;

        [SetUp]
        public void Setup()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
        }

        [TearDown]
        public void Down()
        {
            _mockMessageRepository.Reset();
        }

        [Test]
        public async Task Handle_Should_WhenRequestInValid()
        {
            var handler = new GetUserMessagesQueryHandler(_mockMessageRepository.Object);
            var result = await handler.Handle(new GetUserMessagesQuery(), CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(0);
        }

        [Test]
        public async Task Handle_Should_WhenUserMessagesDoesNot()
        {
            var faker = new Faker("tr");
            var getUserMessageQuery = new GetUserMessagesQuery()
            {
                UserName = faker.Person.UserName
            };
            _mockMessageRepository.Setup(x => x.GetAllAsync(x => x.SenderUserName == getUserMessageQuery.UserName))
                .ReturnsAsync(new List<Message>());

            var handler = new GetUserMessagesQueryHandler(_mockMessageRepository.Object);
            var result = await handler.Handle(getUserMessageQuery, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(0);
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError11);
        }

        [Test]
        public async Task Handle_Should_WhenUserMessagesSuccess()
        {
            var faker = new Faker("tr");
            var getUserMessageQuery = new GetUserMessagesQuery()
            {
                UserName = faker.Person.UserName
            };
            _mockMessageRepository.Setup(x => x.GetAllAsync(x => x.SenderUserName == getUserMessageQuery.UserName))
                .ReturnsAsync(new List<Message>()
                {
                    Message.Create(faker.Random.String(), faker.Random.String(), faker.Random.String(), faker.Random.String(), faker.Random.String())
                });
            var handler = new GetUserMessagesQueryHandler(_mockMessageRepository.Object);
            var result = await handler.Handle(getUserMessageQuery, CancellationToken.None);
            result.Success.Should().BeTrue();
            result.Result.Should().HaveCountGreaterThan(0);
        }
    }
}