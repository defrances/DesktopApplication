using DesktopApplication.Core;

namespace DesktopApplication.Tests;

public sealed class SystemInformationProviderTests
{
    [Fact]
    public void GetCurrent_ReturnsPopulatedValues()
    {
        var info = SystemInformationProvider.GetCurrent();

        Assert.False(string.IsNullOrWhiteSpace(info.UserName));
        Assert.False(string.IsNullOrWhiteSpace(info.MachineName));
        Assert.False(string.IsNullOrWhiteSpace(info.OperatingSystem));
        Assert.False(string.IsNullOrWhiteSpace(info.Framework));
        Assert.False(string.IsNullOrWhiteSpace(info.ProcessArchitecture));
    }
}
