using Bogus;
using FluentAssertions;
using MessageService.Application.Features.Users.BlockUsers.Queries;
using MessageService.Domain.Entities;
using MessageService.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace MessageService.UnitTest.Application.Handlers.Users
{
    public class GetBlockedUsersQueryHandlerTest
    {
        private Mock<IBlockUserRepository> _mockBlockUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockBlockUserRepository = new Mock<IBlockUserRepository>();
        }

        [TearDown]
        public void Down()
        {
            _mockBlockUserRepository.Reset();
        }

        [Test]
        public async Task Handle_Should_ResponseSuccess()
        {
            var faker = new Faker("tr");
            var query = new GetBlockedUsersQuery()
            {
                BlockingUserName = "ali.demir"
            };
            _mockBlockUserRepository.Setup(x => x.GetAllAsync(x => x.Blocking == query.BlockingUserName))
                .ReturnsAsync(new List<BlockUser>()
                {
                    BlockUser.Create(faker.Random.String(), faker.Random.String())
                });

            var handler = new GetBlockedUsersQueryHandler(_mockBlockUserRepository.Object);
            var result = await handler.Handle(query, CancellationToken.None);
            result.Success.Should().BeTrue();
            result.Result.Should().HaveCountGreaterThan(0);
        }
    }
}