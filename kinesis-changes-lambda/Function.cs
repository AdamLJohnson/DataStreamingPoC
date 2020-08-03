using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.ApiGatewayManagementApi;
using Amazon.ApiGatewayManagementApi.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using SharedStuff;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace kinesis_changes_lambda
{
    public class Function
    {
        IAmazonDynamoDB _dynamoDb = new AmazonDynamoDBClient();
        IAmazonApiGatewayManagementApi _apiClient = new AmazonApiGatewayManagementApiClient(new AmazonApiGatewayManagementApiConfig()
        {
            ServiceURL = "****API Gateway Connection URL****"
        });

        public async Task FunctionHandler(KinesisEvent kinesisEvent, ILambdaContext context)
        {
            context.Logger.LogLine($"Beginning to process {kinesisEvent.Records.Count} records...");

            var table = Table.LoadTable(_dynamoDb, Constants.TableName);
            var scanResp = await _dynamoDb.ScanAsync(new ScanRequest()
            {
                TableName = Constants.TableName,
                ProjectionExpression = Constants.ConnectionIdField
            });

            foreach (var record in kinesisEvent.Records)
            {
                context.Logger.LogLine($"Event ID: {record.EventId}");
                context.Logger.LogLine($"Event Name: {record.EventName}");

                string recordData = GetRecordContents(record.Kinesis);
                context.Logger.LogLine($"Record Data:");
                context.Logger.LogLine(recordData);

                var stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(recordData));

                foreach (var item in scanResp.Items)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Position = 0;

                    var connectionId = item[Constants.ConnectionIdField].S;
                    var postConnectionRequest = new PostToConnectionRequest()
                    {
                        ConnectionId = connectionId,
                        Data = stream
                    };

                    try
                    {
                        await _apiClient.PostToConnectionAsync(postConnectionRequest);
                    }
                    catch (AmazonServiceException e)
                    {
                        if (e.StatusCode == HttpStatusCode.Gone)
                        {
                            await table.DeleteItemAsync(connectionId);
                        } 
                        else throw;
                    }
                }
            }

            context.Logger.LogLine("Stream processing complete.");
        }

        private string GetRecordContents(KinesisEvent.Record streamRecord)
        {
            using (var reader = new StreamReader(streamRecord.Data, Encoding.ASCII))
            {
                return reader.ReadToEnd();
            }
        }
    }
}