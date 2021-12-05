# ServiceBusAdmin advanced usage scenarios
Before you start using ServiceBusAdmin you must configure connection string to Azure Service Bus namespace that you will be connecting to. You can do it either by providing the connection string to every ServiceBusAdmin command, or by setting `SEBA_CONNECTION_STRING` environment variable.
```shell
export SEBA_CONNECTION_STRING="<your_connection_string>"
```

Examples below use Service Bus test entities that can be created with following commands
```shell
seba topic create topic1
```
```shell
seba subscription create topic1/sub1
```

## Table of contents
1. [Sending single message to a topic](#1-sending-single-message-to-a-topic)
2. [Sending messages in batch](#2-sending-messages-in-batch)
3. [Filtering peeked messages](#3-filtering-peeked-messages)
4. [Moving messages to dead letter queue of a subscription](#4-moving-messages-to-dead-letter-queue-of-a-subscription)
5. [Moving messages from dead letter queue of a subscription to a topic](#5-moving-messages-from-dead-letter-queue-of-a-subscription-to-a-topic)

### 1. Sending single message to a topic
```shell
seba topic send topic1 -m '{"body":{"someKey":69},"metadata":{"applicationProperties":{"key1":"value1"}}}'
```
JSON schema of the message can be found [here](input-message-schema.json).

### 2. Sending messages in batch
Generate 10 000 sample messages
```shell
for i in {1..10000}; \
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

Send messages in batch
```shell
seba topic send-batch topic1 -i input.json
```
JSON schema of messages (lines in the input file provided to `send-batch` command) can be found [here](input-message-schema.json).

### 3. Filtering peeked messages
Please note that filtering takes place on your machine. Seba peeks requested number of messages (controlled with `-m` option) and then they are being filtered with `jq` tool in your shell. Therefore, if you are processing large number of messages, it's recommended to write them to a file first and then process the file.

Peek and write messages to `output.json` file
```shell
seba subscription peek topic1/sub1 -m 10000 -o all > output.json
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

### 4. Moving messages to dead letter queue of a subscription
Command below receives first 10 messages from subscription `sub1` under topic `topic1` and then sends them to dead letter queue associated with the subscription. 
```shell
seba subscription receive dead-letter topic1/sub1 --max 10
```

### 5. Moving messages from dead letter queue of a subscription to a topic
Command below receives first 5 messages from dead letter queue associated with subscription `sub1` under topic `topic1` and then sends them back to `topic1`.
```shell
seba subscription receive send-to-topic topic1/sub1 -dlq --max 5
```