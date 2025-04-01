using Ocelot.DependencyInjection;
using Ocelot.Middleware;





var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



builder.Services.AddOcelot();
builder.Services.AddLogging();



if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
