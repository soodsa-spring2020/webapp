using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using csye6225.Models;
using Microsoft.Extensions.Options;

namespace csye6225.Services
{
    public interface INotificationService
    {
        void AddToNotificationQueue(string email, IEnumerable<BillResponse> bills);
    }

    public class NotificationService : INotificationService {
         
        private readonly IOptions<Parameters> _config;
        private readonly AmazonSQSClient _client;

        public NotificationService(IOptions<Parameters> config)
        {
            _config = config;
            _client = new AmazonSQSClient();
        }

        public void AddToNotificationQueue(string email, IEnumerable<BillResponse> bills) 
        {
            var sendRequest = new SendMessageRequest(); 
            var appUrl = _config.Value.APP_URL;
            sendRequest.QueueUrl = _config.Value.SQS_URL;
            List<string> l = new List<string>();
            foreach(var b in bills) {
                l.Add(string.Format("http://{0}/v1/bill/{1}", appUrl, b.id));
            }

            sendRequest.MessageBody = JsonSerializer.Serialize(new { email  = email, bill = string.Join(",", l) });
            var sendMessageResponse = _client.SendMessageAsync(sendRequest).Result;
        }
    }
}