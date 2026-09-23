using System.Runtime.InteropServices;

namespace DesktopApplication.Core;

public sealed record SystemInformation(
    string UserName,
    string MachineName,
    string OperatingSystem,
    string Framework,
    string ProcessArchitecture);

public static class SystemInformationProvider
{
    public static SystemInformation GetCurrent()
    {
        return new SystemInformation(
            Environment.UserName,
            Environment.MachineName,
            RuntimeInformation.OSDescription,
            RuntimeInformation.FrameworkDescription,
            RuntimeInformation.ProcessArchitecture.ToString());
    }
}
