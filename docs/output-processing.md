Filtering by body

```shell
seba subscription peek topic1/sub1 -m 100 -o all | jq -c 'select(.body.key1 > 95 and .body.key2.key22 < 196)'
```