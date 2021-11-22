Generate 1000 sample messages
```shell
echo [ > input.json; \
    for i in {1..1000}; \
        do echo {\"messageId\":\"$i\",\"body\":{\"key1\":$i,\"key2\":{\"key21\":\"Value $i\",\"key22\":$(($i*2))}},\"applicationProperties\":{\"prop1\":\"Value $i\",\"prop2\":$(($i*2))}},; \
    done >> input.json; \
echo ] >> input.json
```