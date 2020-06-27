# NServiceBus SQL Database Differences

## Differences

The Subscription table has an additional nullable column `LogicalEndpoint` which is null unless additional configuration is used for identifying logical endpoints. 
The TimeoutEntity table `CorrelationId` column is no longer populated, the value is contained within the `Headers` column instead (which it always was).

NSB 4.7.12 and NSB 7.3.0 endpoints are able to send messages, publish events and defer commands with the same NServiceBus database. NSB 7.3.0 endpoints are able to directly defer commands to NSB 4.7.12 endpoints due to enhancements in the message sending API. Deferred messages in NSB 4.7.12 endpoints are always processed by the same originating queue, but testing of the defer process still worked as previously (including the correlationId header extraction).

## Summary

If the Subscription table `LogicalEndpoint`column is not found the NSB 7.3.0 endpoint will refuse to start. The NSBus 7.3.0 endpoint can be configured to automatically modify the SQL database tables, but given how small the change is it would be better to make the change once via a script.

Other than the additional column the two endpoints were able to co-exist with the same NServiceBus subscription / timeoutentity database.

## Example of TimeoutEntity.Headers

#### NServiceBus 4.7.12 / NServiceBus.NHibernate 4.5.5 
```json
{
"NServiceBus.MessageId":"aae386d4-e3f8-4891-8b00-abe800acdf66",
"NServiceBus.CorrelationId":"aae386d4-e3f8-4891-8b00-abe800acdf66",
"NServiceBus.OriginatingEndpoint":"Endpoint4712",
"$.diagnostics.originating.hostid":"04fc67979d70048e29e55e8c7f4a0956",
"NServiceBus.MessageIntent":"Send",
"NServiceBus.Version":"4.7.12",
"NServiceBus.TimeSent":"2020-06-28 09:29:24:604821 Z",
"NServiceBus.OriginatingMachine":"computer",
"NServiceBus.ContentType":"application/json",
"NServiceBus.EnclosedMessageTypes":"Contracts.Events.IPaymentIntentAdded, Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
"CorrId":"aae386d4-e3f8-4891-8b00-abe800acdf66\\0",
"WinIdName":"",
"NServiceBus.RelatedTo":"bf14dfc5-e05a-47f0-b1bc-abe800acdf3a",
"NServiceBus.ConversationId":"70fc4223-66c7-4684-b2ca-abe800acdee9",
"NServiceBus.IsDeferredMessage":"True",
"NServiceBus.Temporary.DelayDeliveryWith":"00:00:05",
"NServiceBus.Timeout.Expire":"2020-06-28 09:29:29:605811 Z",
"NServiceBus.Timeout.RouteExpiredTimeoutTo":"Endpoint4712@computer",
"NServiceBus.Timeout.ReplyToAddress":"Endpoint4712@computer"
}
```

#### NServiceBus 7.3.0 / NServiceBus.NHibernate 8.4.2

```json
{
"NServiceBus.MessageId":"0d104264-8190-45e9-af9f-abe800a16182",
"NServiceBus.MessageIntent":"Send",
"NServiceBus.RelatedTo":"a564855a-4aab-4bfc-94b5-abe800b1dbd1",
"NServiceBus.ConversationId":"18571368-b994-44f7-a10c-abe800b1db82",
"NServiceBus.CorrelationId":"d18b0acf-07c0-45b7-b6a1-abe800b1dbe7",
"NServiceBus.OriginatingMachine":"computer",
"NServiceBus.OriginatingEndpoint":"Endpoint730",
"$.diagnostics.originating.hostid":"abc0e0c640f2c4314456160d1e9e200e",
"NServiceBus.ReplyToAddress":"Endpoint730@computer",
"NServiceBus.ContentType":"application\/json",
"NServiceBus.EnclosedMessageTypes":"Contracts.Commands.ICheckIfPaymentShouldBeExpired, Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
"NServiceBus.Version":"7.2.0",
"NServiceBus.TimeSent":"2020-06-28 09:47:34:264428 Z",
"NServiceBus.Timeout.RouteExpiredTimeoutTo":"Endpoint4712@computer",
"NServiceBus.Timeout.Expire":"2020-06-28 09:47:39:263427 Z",
"CorrId":"d18b0acf-07c0-45b7-b6a1-abe800b1dbe7\\0"
}
```

### Subscription Table

#### NServiceBus.NHibernate Version 4.5.5

```sql
CREATE TABLE [dbo].[Subscription](
	[SubscriberEndpoint] [varchar](450) NOT NULL,
	[MessageType] [varchar](450) NOT NULL,
	[Version] [varchar](450) NULL,
	[TypeName] [varchar](450) NULL,
PRIMARY KEY CLUSTERED 
(
	[SubscriberEndpoint] ASC,
	[MessageType] ASC
)
```
#### NServiceBus.NHibernate Version 8.4.2

```sql
CREATE TABLE [dbo].[Subscription](
	[SubscriberEndpoint] [varchar](450) NOT NULL,
	[MessageType] [varchar](450) NOT NULL,
	[Version] [varchar](450) NULL,
	[TypeName] [varchar](450) NULL,
	[LogicalEndpoint] [varchar](450) NULL,
PRIMARY KEY CLUSTERED 
(
	[SubscriberEndpoint] ASC,
	[MessageType] ASC
)
```

### TimeoutEntity table

#### NServiceBus.NHibernate Version 4.5.5

```sql
CREATE TABLE [dbo].[TimeoutEntity](
	[Id] [uniqueidentifier] NOT NULL,
	[Destination] [nvarchar](1024) NULL,
	[SagaId] [uniqueidentifier] NULL,
	[State] [varbinary](max) NULL,
	[Time] [datetime] NULL,
	[CorrelationId] [nvarchar](1024) NULL,
	[Headers] [nvarchar](max) NULL,
	[Endpoint] [nvarchar](440) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)
```

#### NServiceBus.NHibernate Version 8.4.2

```sql
CREATE TABLE [dbo].[TimeoutEntity](
	[Id] [uniqueidentifier] NOT NULL,
	[Destination] [nvarchar](1024) NULL,
	[SagaId] [uniqueidentifier] NULL,
	[State] [varbinary](max) NULL,
	[Time] [datetime] NULL,
	[Headers] [nvarchar](max) NULL,
	[Endpoint] [nvarchar](440) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)
```
