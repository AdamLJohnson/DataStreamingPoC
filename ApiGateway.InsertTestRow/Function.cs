using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.SimpleSystemsManagement;
using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement.Model;
using Dapper;
using SharedStuff;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ApiGateway.InsertTestRow
{
    public class Function
    {
        IAmazonSimpleSystemsManagement _parametersClient = new AmazonSimpleSystemsManagementClient();

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var connString = await _parametersClient.GetParameterAsync(new GetParameterRequest(){Name = Constants.AdamSqlConnectionString});
            await using (var connection = new SqlConnection(connString.Parameter.Value))
            {
                await connection.ExecuteAsync("insert into Users (Username, FirstName, LastName) values ('testtest', 'test', 'test')");
            }

            return new APIGatewayProxyResponse()
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
