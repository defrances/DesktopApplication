using System.Net.Security;
using DesktopApplication.Core;

namespace DesktopApplication.Tests;

public sealed class InsecureVendorBulletinClientTests
{
    [Fact]
    public void Marker_IdentifiesIntentionalSkillTestVulnerability()
    {
        Assert.Equal("INTENTIONAL_SKILL_TEST_VULNERABILITY", InsecureVendorBulletinClient.Marker);
    }

    [Fact]
    public void AcceptAnyServerCertificate_ReturnsTrue_WhenTheChainIsInvalid()
    {
        var accepted = InsecureVendorBulletinClient.AcceptAnyServerCertificate(
            null,
            null,
            null,
            SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors);

        Assert.True(accepted);
    }
}
