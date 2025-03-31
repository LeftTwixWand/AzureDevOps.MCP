using AzureDevOps.MCP.ServiceDefaults;
using AzureDevOps.MCP.Web.Components;
using ModelContextProtocol;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddSingleton<IMcpClient>(serviceProvider =>
{
	McpClientOptions mcpClientOptions = new()
	{ ClientInfo = new() { Name = "AspNetCoreSseClient", Version = "1.0.0" } };

	HttpClient httpClient = new()
	{
		BaseAddress = new("https://localhost:7515/sse")  //"https +http://aspnetsseserver" + "/sse")
	};

	McpServerConfig mcpServerConfig = new()
	{
		Id = "AspNetCoreSse",
		Name = "AspNetCoreSse",
		TransportType = TransportTypes.Sse,
		Location = httpClient.BaseAddress.ToString(),
	};

	var mcpClient = McpClientFactory.CreateAsync(mcpServerConfig, mcpClientOptions).GetAwaiter().GetResult();
	return mcpClient;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();