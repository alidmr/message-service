namespace MessageService.Infrastructure.Constants
{
    public static class RabbitMqConstants
    {
        public static readonly string ExchangeName = "MessageServiceExchange";
        
        public static readonly string UserLogRoutingKey = "user-log-route";
        public static readonly string UserLogQueueName = "user-log";

        public static readonly string MessageRoutingKey = "message-route";
        public static readonly string MessageQueueName = "message";
    }
}