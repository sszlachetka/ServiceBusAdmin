## ServiceBusAdmin advanced usage scenarios
If you don't want to provide Service Bus connection string to every ServiceBusAdmin command then use `SEBA_CONNECTION_STRING` environment variable.
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
### 1. Sending messages in batch
#### Generate 10 000 sample messages
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

#### Send messages in batch
```shell
seba topic send-batch topic1 -i input.json
```
JSON schema of messages (lines in input file provided to `send-batch` command) can be found [here](send-batch-input-schema.json).

### 2. Filtering peeked messages
Please note that filtering takes place on your machine. Seba peeks requested number of messages (controlled with `-m` option) and then they are being filtered with `jq` tool in your shell. Therefore, if you are processing large number of messages, it's recommended to write them to a file first and then process the file.

#### Peek and write messages to `output.json` file
```shell
seba subscription peek topic1/sub1 -m 10000 -o all > output.json
```
Filtering scenarios described below use data from `output.json` file.

#### Filtering by body
```shell
cat output.json | jq -c 'select(.body.key1 > 3 and .body.key2.key21 < 10)'
```

### Filtering by enqueued time range
```shell
cat output.json | jq -c --arg from '2021-12-04T14:51:29.318' --arg to '2021-12-04T14:51:29.319' 'select(.metadata.enqueuedTime | . >= $from and . < $to)'
```