Generate 10 000 sample messages
```shell
for i in {1..10000}; \
    do echo {\"messageId\":\"$i\",\"body\":{\"key1\":$i,\"key2\":{\"key21\":\"Value $i\",\"key22\":$(($i*2))}},\"applicationProperties\":{\"prop1\":\"Value $i\",\"prop2\":$(($i*2))}}; \
done > input.json;
```