# Kinesis Data Streaming PoC

This was a proof of concept I created to learn about streaming changes from MSSQL to a Kinesis Data Stream

## Subprojects

### ApiGateway.Client

This is a Blazor WASM application to connect to the AWS API Gateway.

### ApiGateway.DeleteRows

A lambda function that deletes rows from the database.

### ApiGateway.InsertTestRow

A lambda function that inserts a test row into the database.

### ApiGateway.OnConnect

A lambda function that is called when the client application connects to the AWS API Gateway.

### ApiGateway.OnDisconnect

A lambda function that is called when the client application disconnects from the AWS API Gateway.

### kinesis-changes-lambda

A lambda function that subscribes to the Kinesis Data Stream

### SharedStuff

Shared classes

## Architecture Diagram

![Architecture Diagram](https://raw.githubusercontent.com/AdamLJohnson/DataStreamingPoC/master/AWS%20Datastreams%20PoC.png)