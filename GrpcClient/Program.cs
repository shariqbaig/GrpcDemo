using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    class Program
    {
        static async Task Main()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var greeterClient = new Greeter.GreeterClient(channel);
            var customerClient = new Customer.CustomerClient(channel);

            var greeterInput = new HelloRequest
            {
                Name = "John Doe"
            };

            var customerInput = new CustomerLookupModel
            {
                UserId = 2
            };

            var greeterReply = await greeterClient.SayHelloAsync(greeterInput);
            var customerReply = await customerClient.GetCustomerInfoAsync(customerInput);

            Console.WriteLine(greeterReply.Message);
            Console.WriteLine($"{ customerReply.FirstName } { customerReply.LastName }");
            Console.WriteLine();
            
            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{ currentCustomer.FirstName } { currentCustomer.LastName }: { currentCustomer.EmailAddress }");
                }
            }

            Console.ReadLine();
        }
    }
}
