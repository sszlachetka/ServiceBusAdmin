
Generate test messages
```
time for i in {1..10}; do echo $i; ./seba topic send topic1 -b "{\"key\":$i}"; done
```