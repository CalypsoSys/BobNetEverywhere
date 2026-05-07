using bobweb;

namespace bobweb.tests;

public class AppSettingsTests
{
    [Fact]
    public void DefaultsUseRepoRelativeLogPaths()
    {
        var settings = new AppSettings();

        Assert.Equal("logs/access.log", settings.GetAccessLogPath());
        Assert.Equal("logs/errors.log", settings.GetErrorLogPath());
    }
}
