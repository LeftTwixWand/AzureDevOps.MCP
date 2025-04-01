using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AzureDevOps.MCP.ApiService.Tools;

[McpServerToolType]
public class AzureDevOpsTool
{
	private static readonly string _organizationUrl = "https://dev.azure.com/your-org";

	// Personal Access Token (PAT) for authentication
	private static readonly string _personalAccessToken = "your-pat";

	[McpServerTool, Description("Returns list of Azure DevOps repositories in the organization")]
	public async Task<IEnumerable<string>> GetRepositoriesAsync()
	{
		VssConnection connection = new(new Uri(_organizationUrl), new VssBasicCredential(string.Empty, _personalAccessToken));
		GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

		IEnumerable<GitRepository> repositories = await gitClient.GetRepositoriesAsync();

		return repositories.Select(repositories => repositories.Name);
	}
}