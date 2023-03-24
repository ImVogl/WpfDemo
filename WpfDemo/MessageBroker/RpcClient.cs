namespace WpfDemo.MessageBroker;

using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

/// <summary>
/// RabbitMq request-response client.
/// </summary>
public class RpcClient : IDisposable
{
    /// <summary>
    /// Routing key.
    /// </summary>
    private const string RoutingKey = "demo_key";

    /// <summary>
    /// RabbitMQ connection.
    /// </summary>
    private readonly IConnection _connection;

    /// <summary>
    /// Channel for AMQP model.
    /// </summary>
    private readonly IModel _channel;

    /// <summary>
    /// Server queue name.
    /// </summary>
    private readonly string _replyQueueName;

    /// <summary>
    /// Queue with messages.
    /// </summary>
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

    /// <summary>
    /// Initialize new instance of <see cref="RpcClient"/>.
    /// </summary>
    public RpcClient()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _replyQueueName = _channel.QueueDeclare().QueueName;
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) =>
        {
            if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;

            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(response);
        };

        _channel.BasicConsume(consumer: consumer, queue: _replyQueueName, autoAck: true);
    }

    /// <summary>
    /// Modification of text on server side.
    /// </summary>
    /// <param name="text">Text to modification.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>Modified text.</returns>
    public Task<string> ModifyTextAsync(string text, CancellationToken cancellationToken = default)
    {
        var props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replyQueueName;
        var messageBytes = Encoding.UTF8.GetBytes(text);
        var tcs = new TaskCompletionSource<string>();
        _callbackMapper.TryAdd(correlationId, tcs);

        _channel.BasicPublish(exchange: string.Empty,
                             routingKey: RoutingKey,
                             basicProperties: props,
                             body: messageBytes);

        cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));
        return tcs.Task;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _connection.Close();
    }
}
