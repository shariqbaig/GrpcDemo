using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;
        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();

            if (request.UserId == 1)
            {
                output.FirstName = "Papa";
                output.LastName = "Johnny";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "John";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "Cameron";
                output.LastName = "White";
            }

            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName = "Papa",
                    LastName = "Johnny",
                    EmailAddress = "papajohnny@gmail.com"
                },
                new CustomerModel
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "johndoe@gmail.com"
                },
                new CustomerModel
                {
                    FirstName = "Cameron",
                    LastName = "White",
                    EmailAddress = "cameronwhite@gmail.com"
                }
            };

            foreach (var customer in customers)
            {
                await Task.Delay(2000);
                await responseStream.WriteAsync(customer);
            }
        }
     }
}