using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Queue.Sender.Sample
{
    public class Program
    {
        const string QueueConnectionString = "Endpoint=sb://geekburgerfiapmba.servicebus.windows.net/;SharedAccessKeyName=ProductPolicy;SharedAccessKey=BRMloS34TUSJjQ0vumsWbFpVuSKQTo8jO+ASbDtjsJw=";

        const string QueuePath = "ProductChanged";

        private static IQueueClient _queueClient;

        private static IList<Task> PendingCompleteTasks;

        public static void Main(string[] args)
        {
            PendingCompleteTasks = new List<Task>();

            if (args.Length <= 0 || args[0] == "sender")
            {
                ReceiveMessagesAsync().GetAwaiter().GetResult();
                //SendMessagesAsync().GetAwaiter().GetResult();
                Console.WriteLine("messages were sent");
            }
            else if (args[0] == "receiver")
            {
                ReceiveMessagesAsync().GetAwaiter().GetResult();
                Console.WriteLine("messages were received");
            }
            else
            {
                Console.WriteLine("nothing to do");
            }

            Console.ReadLine();
        }

        // Metodo que envia as mensagens para a fila.
        private static async Task SendMessagesAsync()
        {
            _queueClient = new QueueClient(QueueConnectionString, QueuePath);

            var messages = "Hi,Hello,Hey,How are you,Be Welcome"
                .Split(',')
                .Select(msg =>
                {

                    Console.WriteLine($"Will send message: {msg}");
                    return new Message(Encoding.UTF8.GetBytes(msg));

                })
                .ToList();

            var sendTask = _queueClient.SendAsync(messages);
            await sendTask;

            CheckCommunicationExceptions(sendTask);

            var closeTask = _queueClient.CloseAsync();
            await closeTask;

            CheckCommunicationExceptions(closeTask);

        }

        // Metodo que recebe as mensagens da fila.
        private static async Task ReceiveMessagesAsync()
        {
            _queueClient = new QueueClient(QueueConnectionString, QueuePath, ReceiveMode.PeekLock);

            _queueClient.RegisterMessageHandler(MessageHandler, new MessageHandlerOptions(ExceptionHandler) { AutoComplete = false });
            Console.ReadLine();

            Console.WriteLine($" Request to close async. Pending tasks: {PendingCompleteTasks.Count()}");
            await Task.WhenAll(PendingCompleteTasks);

            Console.WriteLine($"All pending tasks were completed");

            var closeTask = _queueClient.CloseAsync();
            await closeTask;

            CheckCommunicationExceptions(closeTask);
        }

        #region :: Handlers ::
        private static async Task MessageHandler(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received message:{Encoding.UTF8.GetString(message.Body)}");

            if (cancellationToken.IsCancellationRequested || _queueClient.IsClosedOrClosing)
            {
                return;
            }

            Console.WriteLine($"task {PendingCompleteTasks.Count()}");

            Task PendingTask;
            lock (PendingCompleteTasks)
            {
                PendingCompleteTasks.Add(_queueClient.CompleteAsync(message.SystemProperties.LockToken));
                PendingTask = PendingCompleteTasks.LastOrDefault();
            }
            Console.WriteLine($"calling complete for task {PendingCompleteTasks.Count()}");
            await PendingTask;

            Console.WriteLine($"remove task {PendingCompleteTasks.Count()} from task queue");
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionHandler(ExceptionReceivedEventArgs exceptionArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionArgs.Exception}.");
            var context = exceptionArgs.ExceptionReceivedContext;

            Console.WriteLine($"Endpoint:{context.Endpoint}, Path:{context.EntityPath}, Action:{context.Action}");
            return Task.CompletedTask;
        }

        #endregion

        #region :: Exceptions ::

        private static bool CheckCommunicationExceptions(Task task)
        {
            if (task.Exception == null || task.Exception.InnerExceptions.Count == 0)
            {
                return true;
            }

            task.Exception.InnerExceptions
                .ToList()
                .ForEach(innerException =>
                {
                    Console.WriteLine($"Error in SendAsync task: {innerException.Message}. Details: {innerException.StackTrace}");

                    if (innerException is ServiceBusCommunicationException)
                    {
                        Console.WriteLine("Connection Problem with Host");
                    }

                });

            return false;
        }

        #endregion

    }
}
