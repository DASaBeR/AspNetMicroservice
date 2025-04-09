using Ocelot.DependencyInjection;
using Ocelot.Middleware;





var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOcelot();
builder.Services.AddLogging();
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
