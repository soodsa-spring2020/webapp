using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using csye6225.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace csye6225.Services
{
    public class PollQueueWorker : BackgroundService
    {
        private readonly IOptions<Parameters> _config;

        public PollQueueWorker(IOptions<Parameters> config)
        {
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                Console.WriteLine("Polling Queue");
                await PollQueue();
                await Task.Delay(10000, stopToken);
            }
        }

        private async Task PollQueue()
        {
            using (var sqs = new AmazonSQSClient())
            {
                const int maxMessages = 10; 
                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = _config.Value.SQS_URL, 
                    MaxNumberOfMessages = maxMessages, 
                    AttributeNames = new List<string> { "All" },
                    WaitTimeSeconds = 5 
                };

                var receiveMessageResponse = await sqs.ReceiveMessageAsync(receiveMessageRequest);

                if (receiveMessageResponse.Messages != null)
                {
                    foreach (var message in receiveMessageResponse.Messages)
                    {
                        Console.WriteLine("PollQueue " + message.MessageId);
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += (obj, e) => ProcessMessage(message);
                        worker.RunWorkerAsync();
                    }
                }
                else
                {
                    Console.WriteLine("No messages on queue");
                }
            }
        }

        private async Task ProcessMessage(Message message)
        {
            Console.WriteLine("ProcessMessage " + message.Body);

            using (var sns = new AmazonSimpleNotificationServiceClient())
            {
                PublishRequest publishReq = new PublishRequest()
                {
                    TargetArn = _config.Value.SNS_TOPIC_ARN,
                    Message = message.Body
                };

                await sns.PublishAsync(publishReq);

                using (var sqs = new AmazonSQSClient())
                {
                    var deleteRequest = new DeleteMessageRequest(_config.Value.SQS_URL, message.ReceiptHandle);
                    await sqs.DeleteMessageAsync(deleteRequest);
                    Console.WriteLine("Processed message id: {0}", message.MessageId);
                }
            }
        }
    }
}