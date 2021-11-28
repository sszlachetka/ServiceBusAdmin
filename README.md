## Goal
Manage messages in your Azure Service Bus namespace with handy CLI. Send, peek and receive messages from your Service Bus entities.

### Quick start
1. Install ServiceBusAdmin .NET tool from [nuget](https://www.nuget.org/packages/ServiceBusAdmin)
    ```shell
    dotnet tool install --global ServiceBusAdmin
    ```
2. Configure connection string to your Service Bus Namespace with environment variable
     ```shell
    export SEBA_CONNECTION_STRING="<service_bus_connection_string>"
    ```
3. Verify your connection string
     ```shell
    seba props
    ```
   If connection string you configured is valid then you should see output similar to
     ```json
    {"NamespaceName":"ssz-playground","CreatedTime":"2021-11-08T09:24:12.41+00:00","ModifiedTime":"2021-11-09T09:23:31.117+00:00"}
    ```
4. List topics
   ```shell
   seba topic list
    ```
5. List subscriptions
   ```shell
   seba subscription list <topic_name>
    ```
6. Peek messages from a subscription's dead letter queue
   ```shell
   seba subscription peek <topic_name>/<subscription_name> -dlq --max 20
    ```
   This will return only messages' metadata. If you need their body as well, then use additional option `--output-content all`
7. Every command supports `--help` option. Use it to learn about all supported options, arguments and sub-commands.
   ```shell
   seba topic --help
   seba subscription --help
   seba subscription receive --help
    ```
