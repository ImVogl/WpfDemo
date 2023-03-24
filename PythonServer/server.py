import pika

routing_key = "demo_key"
connection = pika.BlockingConnection(pika.ConnectionParameters(host = "localhost"))
channel = connection.channel()
channel.queue_declare(queue = routing_key)

def process(body: str) -> str:
    converted_body = body.decode("utf-8")
    return f"changed_{converted_body}"

def on_request(ch, method, props, body):
    print("Recived message")
    ch.basic_publish(exchange = "", routing_key = props.reply_to, properties = pika.BasicProperties(correlation_id = props.correlation_id), body = process(body))
    ch.basic_ack(delivery_tag = method.delivery_tag)

channel.basic_qos(prefetch_count = 1)
channel.basic_consume(queue = routing_key, on_message_callback = on_request)
channel.start_consuming()