using bobweb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace bobweb.tests;

public class ItemStoragePathProviderTests
{
    [Fact]
    public void GetDataPathDefaultsToContentRootDataFolder()
    {
        var environment = new TestWebHostEnvironment { ContentRootPath = "/sample/app" };
        var provider = new ItemStoragePathProvider(Options.Create(new AppSettings()), environment);

        string dataPath = provider.GetDataPath();

        Assert.Equal(Path.Combine("/sample/app", "data"), dataPath);
    }

    [Fact]
    public void GetDataPathResolvesConfiguredRelativePathFromContentRoot()
    {
        var environment = new TestWebHostEnvironment { ContentRootPath = "/sample/app/bobweb" };
        var provider = new ItemStoragePathProvider(
            Options.Create(new AppSettings { FileSaveLocation = "../data" }),
            environment);

        string dataPath = provider.GetDataPath();

        Assert.Equal(Path.GetFullPath("/sample/app/data"), dataPath);
    }

    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "bobweb.tests";
        public IFileProvider ContentRootFileProvider { get; set; } = default!;
        public string ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
        public string EnvironmentName { get; set; } = Microsoft.Extensions.Hosting.Environments.Production;
        public string WebRootPath { get; set; } = Directory.GetCurrentDirectory();
        public IFileProvider WebRootFileProvider { get; set; } = default!;
    }
}
