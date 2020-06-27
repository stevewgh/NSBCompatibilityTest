# NSBCompatibilityTest
Checking the compatibility between NSB 4.7.12 and NSB 7.3.0 when message subscriptions and deferrals are used via a shared NServiceBus SQL database.

[Summary of Differences](https://github.com/stevewgh/NSBCompatibilityTest/blob/master/SQL.md)

## Message Flow Used

1. Using the TestConsole, a `IAddPaymentIntent` command is sent to the NSB 4.7.12 endpoint to begin the process
2. The endpoint handles the command and publishes `IPaymentIntentAdded`
3. Both the NSB 4.7.12 and NSB 7.3.0 endpoints subscribe to the event via the NServiceBus SQL Database subscription table
4. The NSB 4.7.12 endpoint handles the `IPaymentIntentAdded` event and writes to the console to say that it has done so
5. At the same time, the NSB 7.3.0 endpoint handles the `IPaymentIntentAdded` event and sends a deferred `ICheckIfPaymentShouldBeExpired` command back to the NSB 4.7.12 endpoint
6. Once the derral expires the command is delivered to the NSB 4.7.12 endpoint which handles the command and writes the correlationId to the console

```
+-------------------+
|                   |
|                   +------------------+
|    NSB 4.7.12     |                  |
|                   |                  |
|                   |       +----------v----------+
+-------------------+       |                     |
                            |    NServiceBus      |
                            |    SQL Database     |
+-------------------+       |                     |
|                   |       +----------+----------+
|                   |                  ^
|     NSB 7.3.0     |                  |
|                   +------------------+
|                   |
+-------------------+
```

## Instructions

1. You'll need a local SQL Server database called Nservicebus
2. Run Endpoint4712\bin\Debug\net48\NServiceBus.Host.exe
3. Run Endpoint730\bin\Debug\net48\Endpoint730.exe
4. Run TestConsole\bin\Debug\net48\TestConsole.exe (this will send a command to the Endpoint4712 endpoint and then close)
