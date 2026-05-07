using bobweb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace bobweb.tests;

public class StartupTests
{
    [Fact]
    public void ConfigureDoesNotRegisterHttpsRedirectionMiddleware()
    {
        string startupSource = File.ReadAllText(FindRepoFile("bobweb", "Startup.cs"));

        Assert.DoesNotContain("UseHttpsRedirection", startupSource);
    }

    [Fact]
    public async Task ConfigureServicesRegistersConfiguredCorsOrigins()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AppSettings:AllowedOrigins:0"] = "https://static.example.test"
            })
            .Build();
        var services = new ServiceCollection();

        new Startup(configuration).ConfigureServices(services);
        using ServiceProvider provider = services.BuildServiceProvider();
        var policyProvider = provider.GetRequiredService<ICorsPolicyProvider>();

        CorsPolicy? policy = await policyProvider.GetPolicyAsync(new DefaultHttpContext(), null);

        Assert.NotNull(policy);
        Assert.Contains("https://static.example.test", policy.Origins);
    }

    private static string FindRepoFile(params string[] pathParts)
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            string candidate = Path.Combine([current.FullName, .. pathParts]);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            current = current.Parent;
        }

        throw new FileNotFoundException("Could not find repository file.", Path.Combine(pathParts));
    }
}
