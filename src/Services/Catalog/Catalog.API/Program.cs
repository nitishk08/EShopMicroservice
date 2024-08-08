
using BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Add service to the container
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(Config =>
{
Config.RegisterServicesFromAssembly(assembly);
Config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the Http request pipeline
app.MapCarter();
app.Run();
