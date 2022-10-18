﻿using MessageService.Domain.Entities;
using MessageService.Domain.Repositories.Base;

namespace MessageService.Domain.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
}