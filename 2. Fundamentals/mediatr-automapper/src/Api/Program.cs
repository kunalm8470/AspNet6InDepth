using Api.Middlewares;
using Application.v1.Persons;
using Application.v1.Persons.Commands.UpdatePerson;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

// Register the repositories
builder.Services.AddSingleton<IInMemoryPersonRepository, InMemoryPersonRepository>();

// Register MediatR handlers
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblyContaining<UpdatePersonCommandHandler>();
});

// Register AutoMapper profiles
builder.Services.AddAutoMapper(typeof(PersonMappingProfile));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
