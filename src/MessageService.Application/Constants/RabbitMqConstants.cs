namespace MessageService.Application.Constants
{
    public static class RabbitMqConstants
    {
        public static string ExchangeName = "MessageServiceExchange";
        
        public static string UserLogRoutingKey = "user-log-route";
        public static string UserLogQueueName = "user-log";

        public static string MessageRoutingKey = "message-route";
        public static string MessageQueueName = "message";
    }
}