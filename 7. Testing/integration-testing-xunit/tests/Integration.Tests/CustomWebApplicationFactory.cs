using Api;
using Api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor serviceDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(PostgresConnectionFactory));

            // Removed the actual postgres connection factory which uses the real connection string
            services.Remove(serviceDescriptor);

            /*
             * Configuring the test connection string
            */
            string connectionString = "Host=localhost;Port=5433;Username=kunal;Password=root;Database=integrationtesting";

            services.AddTransient(implementationFactory =>
            {
                return new PostgresConnectionFactory(connectionString);
            });

            // To insert seed data 
            ServiceProvider sp = services.BuildServiceProvider();

            using IServiceScope scope = sp.CreateScope();

            IPersonsRepository repository = scope.ServiceProvider.GetService<IPersonsRepository>();

            repository.AddPersonAsync(new Api.Models.Person
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 10
            }, CancellationToken.None).GetAwaiter().GetResult();

            repository.AddPersonAsync(new Api.Models.Person
            {
                FirstName = "Jane",
                LastName = "Doe",
                Age = 11
            }, CancellationToken.None).GetAwaiter().GetResult();

            repository.AddPersonAsync(new Api.Models.Person
            {
                FirstName = "JJ",
                LastName = "Doe",
                Age = 12
            }, CancellationToken.None).GetAwaiter().GetResult();
        });
    }
}
