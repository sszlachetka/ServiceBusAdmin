# ServiceBusAdmin advanced usage scenarios
Before you start using ServiceBusAdmin you must configure connection string to Azure Service Bus namespace that you will be connecting to. You can do it either by providing the connection string to every ServiceBusAdmin command, or by setting `SEBA_CONNECTION_STRING` environment variable.
```shell
export SEBA_CONNECTION_STRING="<your_connection_string>"
```

Examples below use Service Bus test entities that can be created with following commands
```shell
seba queue create queue1
```

```shell
seba topic create topic1
```

```shell
seba subscription create topic1/sub1
```

## Table of contents
1. [Send a single message](#1-send-a-single-message)
2. [Send messages in batch mode](#2-send-messages-in-batch-mode)
3. [Peek messages](#3-peek-messages)
4. [Filter peeked messages](#4-filter-peeked-messages)
5. [Receive messages](#5-receive-messages)

### 1. Send a single message
A massage can be send to a queue
```shell
seba send queue1 -m '{"body":{"someKey":69},"metadata":{"applicationProperties":{"key1":"value1"}}}'
```

or to a topic
```shell
seba send topic1 -m '{"body":{"someKey":69},"metadata":{"applicationProperties":{"key1":"value1"}}}'
```

JSON schema of the message can be found [here](input-message-schema.json).

### 2. Send messages in batch mode
Generate 2 000 sample messages and write them to a file
```shell
for i in {1..2000}; \
    do echo '{"metadata":{"messageId":"'${i}'","applicationProperties":{"prop1":'$((${i}*2))'}},"body":{"key1":'$((${i}*3))',"key2":{"key21":'$((${i}*4))'}}}'; \
done > input.json;
```

Each message has following format
```json
{
  "metadata": {
    "messageId": "100",
    "applicationProperties": {
      "prop1": 200
    }
  },
  "body": {
    "key1": 300,
    "key2": {
      "key21": 400
    }
  }
}
```

Messages can be send in batch to a queue
```shell
seba send-batch queue1 -i input.json
```

or to a topic
```shell
seba send-batch topic1 -i input.json
```

JSON schema of messages (lines in the input file provided to `send-batch` command) is the same as the schema of message supported by `send` command and can be found [here](input-message-schema.json).

Verify number of messages in a queue 
```shell
seba queue props queue1
```

or in a subscription
```shell
seba subscription props topic1/sub1
```

### 3. Peek messages
Number of peeked messages can be controlled with `-m|--max` option (by default it's 10). You can peek message body, metadata or both at a time. This is controlled with `-o|--output-content` option, which can take one of following values: `metadata`, `body`, `all`.

Peek first 20 messages and return only metadata
```shell
seba subscription peek topic1/sub1 -m 20 -o metadata
```

To peek messages from specific sequence number use `-fs|--from-sequence-number` option
```shell
seba subscription peek topic1/sub1 -fs 1995
```

### 4. Filter peeked messages
Please note that filtering takes place on your machine. ServiceBusAdmin peeks requested number of messages (controlled with `-m|--max` option) and then they are being filtered with `jq` tool in your shell. Therefore, if you are processing large number of messages, it's recommended to write them to a file first and then process the file.

Peek and write messages to `output.json` file
```shell
seba subscription peek topic1/sub1 -m 2001 -o all > output.json
```
Filtering scenarios described below use data from `output.json` file.

Filtering by body
```shell
cat output.json | jq -c 'select(.body.key1 > 3 and .body.key2.key21 < 10)'
```

Filtering by enqueued time - inline
```shell
cat output.json | jq -c 'select(.metadata.enqueuedTime >= "2021-12-05")'
```

Filtering by enqueued time range - arguments
```shell
cat output.json | jq -c --arg from '2021-12-04T14:51:29.318' --arg to '2021-12-04T14:51:29.319' 'select(.metadata.enqueuedTime | . >= $from and . < $to)'
```

Filtering by message sequence number
```shell
cat output.json | jq -c 'select(.metadata.sequenceNumber == 999)'
```

Filtering by message ID
```shell
cat output.json | jq -c 'select(.metadata.messageId == "777")'
```

### 5. Receive messages
Messages are received in [peek-lock mode](https://docs.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock). `receive` command supports following sub-commands, which you can use to decide what will happen with received messages
- `console` - print message to the console and complete the message
- `dead-letter` - move message to dead-letter queue
- `send` - send copy of a message back to the topic and complete the original message from DLQ

Move first 1000 messages from subscription to dead-letter queue 
```shell
seba subscription receive dead-letter topic1/sub1 --max 1000
```

Do the same as above but perform dead-letter operation concurrently on up to 20 messages at a time. This option significantly improves throughput of messages. Please note that when messages are moved to DLQ, then their sequence numbers are not changed, so they can be processed concurrently without impacting sequence numbers.
```shell
seba subscription receive dead-letter topic1/sub1 --max 1000 --message-handling-concurrency-level 20
```

Move first 1000 messages from subscription's dead-letter queue back to the topic. Handle concurrently up to 20 messages at a time. Please note that when messages are moved from DLQ to other Service Bus entities, then they get new sequence numbers, so the order in which messages are sent is meaningful. Using `--message-handling-concurrency-level` option when moving messages from DLQ to other Service Bus entity may change the order of messages.
```shell
seba subscription receive send topic1/sub1 -dlq --max 1000 --message-handling-concurrency-level 20
```

Move three messages with sequence numbers 1699, 1799 and 1899 from subscription's dead-letter queue back to the topic.
```shell
seba subscription receive send topic1/sub1 -dlq --max 1000 --message-handling-concurrency-level 20 --handle-sequence-numbers 1699,1799,1899
```