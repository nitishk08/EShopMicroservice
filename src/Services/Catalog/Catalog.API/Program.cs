
var builder = WebApplication.CreateBuilder(args);

// Add service to the container
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(Config =>
{
    Config.RegisterServicesFromAssembly(assembly);
    Config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    Config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // use this one either below one

var app = builder.Build();

// Configure the Http request pipeline
app.MapCarter();

//Global Exception handle
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception is null)
//        {
//            return;
//        }

//        var problemDetails = new ProblemDetails
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";

//        await context.Response.WriteAsJsonAsync(problemDetails);

//    });
//});

// In Place

app.UseExceptionHandler(options => { }); // If you are going to use just above commented code then remove this line
app.Run();
