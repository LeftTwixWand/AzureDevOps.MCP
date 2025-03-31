using AzureDevOps.MCP.ApiService;
using AzureDevOps.MCP.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddMcpServer().WithToolsFromAssembly();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapDefaultEndpoints();
app.MapGet("/", () => $"Hello World! {DateTime.Now}");
app.MapMcpSse();

app.Run();