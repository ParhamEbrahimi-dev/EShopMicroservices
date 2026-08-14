
var builder = WebApplication.CreateBuilder(args);
//add services to the container

var catalogAssembly = typeof(Program).Assembly;

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(catalogAssembly);
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(catalogAssembly);

builder.Services.AddCarter(configurator: config =>
{
    var modules = catalogAssembly.GetTypes()
        .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

    config.WithModules(modules);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();
//configure HTTP request pipeline

app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
