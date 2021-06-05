namespace MinhaLoja.Core.Settings
{
    public class ServiceBusSettings
    {
        public Queue EventQueue { get; set; }
        public string ErrorQueueName { get; set; }
    }

    public class Queue
    {
        public string QueueName { get; set; }
        public string ConnectionStringSend { get; set; }
        public string ConnectionStringListen { get; set; }
    }
}
