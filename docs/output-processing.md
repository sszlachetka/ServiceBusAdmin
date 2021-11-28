Filtering by body
```shell
seba subscription peek topic1/sub1 -m 100 -o all | jq -c 'select(.body.key1 > 95 and .body.key2.key22 < 196)'
```

Filtering by enqueued time between
```shell
seba subscription peek topic1/sub1 -m 20 | jq -c --arg from '2021-11-28T18:12:58' --arg to '2021-11-28T19:30:20' 'select(.enqueuedTime | . >= $from and . < $to)'
```