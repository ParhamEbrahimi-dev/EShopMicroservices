var builder = WebApplication.CreateBuilder(args);


//add services to the container
var catalogAssembly = typeof(Program).Assembly;

builder.Services.AddCarter(configurator: config =>
{
    var modules = catalogAssembly.GetTypes()
        .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

    config.WithModules(modules);
});


builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(catalogAssembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();


//configure HTTP request pipeline

app.MapCarter();

app.Run();
