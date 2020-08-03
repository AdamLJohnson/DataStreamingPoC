using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using SharedStuff;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ApiGateway.OnDisconnect
{
    public class Function
    {
        IAmazonDynamoDB _dynamoDb = new AmazonDynamoDBClient();

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var table = Table.LoadTable(_dynamoDb, Constants.TableName);
            _ = table.DeleteItemAsync(request.RequestContext.ConnectionId);

            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
