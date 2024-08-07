var builder = WebApplication.CreateBuilder(args);

// Add service to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(Config =>
{
    Config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the Http request pipeline
app.MapCarter();
app.Run();
