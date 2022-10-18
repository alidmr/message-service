using Bogus;
using FluentAssertions;
using MessageService.Application.Constants;
using MessageService.Application.Events.Message;
using MessageService.Application.Features.Messages.Send.Commands;
using MessageService.Application.Services.RabbitMq;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Messages
{
    public class SendMessageCommandHandlerTest
    {
        private Mock<IMessageRepository> _mockMessageRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IRabbitMqService> _mockRabbitMqService;

        [SetUp]
        public void Setup()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRabbitMqService = new Mock<IRabbitMqService>();
        }

        [TearDown]
        public void Down()
        {
            _mockUserRepository.Reset();
            _mockMessageRepository.Reset();
            _mockRabbitMqService.Reset();
        }

        [Test]
        public async Task Handle_Should_WhenRequestInValid()
        {
            var sendMessageCommand = new SendMessageCommand();
            var handler = new SendMessageCommandHandler(_mockMessageRepository.Object, _mockUserRepository.Object, _mockRabbitMqService.Object);
            var result = await handler.Handle(sendMessageCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.Should().HaveCountGreaterThan(1);
        }

        [Test]
        public async Task Handle_Should_WhenReceiverUserNull()
        {
            var faker = new Faker("tr");
            var sendMessageCommand = new SendMessageCommand()
            {
                Receiver = faker.Random.String(),
                Sender = faker.Random.String(),
                MessageContent = faker.Random.String(),
                ReceiverUserName = faker.Random.String(),
                SenderUserName = faker.Random.String()
            };
            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == sendMessageCommand.ReceiverUserName))
                .ReturnsAsync((User) null!);

            var handler = new SendMessageCommandHandler(_mockMessageRepository.Object, _mockUserRepository.Object, _mockRabbitMqService.Object);
            var result = await handler.Handle(sendMessageCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
            result.Messages.First().Message.Should().Be(ApplicationErrorMessage.ApplicationError7);
        }

        [Test]
        public async Task Handle_Should_WhenSendMessageSuccess()
        {
            var faker = new Faker("tr");
            var sendMessageCommand = new SendMessageCommand()
            {
                Receiver = faker.Random.String(),
                Sender = faker.Random.String(),
                MessageContent = faker.Random.String(),
                ReceiverUserName = faker.Random.String(),
                SenderUserName = faker.Random.String()
            };

            var user = User.Create(faker.Random.String(), faker.Random.String(), faker.Random.String());

            _mockUserRepository.Setup(x => x.GetAsync(x => x.UserName == sendMessageCommand.ReceiverUserName))
                .ReturnsAsync(user);

            var messageEvent = new MessageCreatedEvent(sendMessageCommand.Sender, sendMessageCommand.Receiver, sendMessageCommand.MessageContent, sendMessageCommand.SenderUserName, sendMessageCommand.ReceiverUserName);

            _mockRabbitMqService.Setup(x => x.Publish(messageEvent, "test-queue", "test-route"));

            var handler = new SendMessageCommandHandler(_mockMessageRepository.Object, _mockUserRepository.Object, _mockRabbitMqService.Object);
            var result = await handler.Handle(sendMessageCommand, CancellationToken.None);
            result.Success.Should().BeTrue();
        }
    }
}