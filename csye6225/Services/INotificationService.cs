using Amazon.SQS;
using Amazon.SQS.Model;
using csye6225.Models;
using Microsoft.Extensions.Options;

namespace csye6225.Services
{
    public interface INotificationService
    {
        void AddToNotificationQueue(string email, string bills);
    }

    public class NotificationService : INotificationService {
         
        private readonly IOptions<Parameters> _config;
        private readonly AmazonSQSClient _client;

        public NotificationService(IOptions<Parameters> config) {
            _config = config;
            _client = new AmazonSQSClient();
        }

        public void AddToNotificationQueue(string email, string bills) 
        {
            var sendRequest = new SendMessageRequest(); 
            sendRequest.QueueUrl = _config.Value.SQS_URL;
            sendRequest.MessageBody = "{ 'email' : '" + email + "','bill' : '" + bills + "'}";
            var sendMessageResponse = _client.SendMessageAsync(sendRequest).Result;
        }
 
    }
}