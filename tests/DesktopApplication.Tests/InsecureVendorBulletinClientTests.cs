using System.Net.Security;
using DesktopApplication.Core;

namespace DesktopApplication.Tests;

public sealed class InsecureVendorBulletinClientTests
{
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("TestId", "TC-UNIT-TLS-MARKER")]
    public void Marker_IdentifiesValidatedTlsPath()
    {
        Assert.Equal("TLS_CERTIFICATE_VALIDATED", InsecureVendorBulletinClient.Marker);
    }

    [Fact]
    [Trait("Category", "Regression")]
    [Trait("TestId", "TC-REG-TLS-CALLBACK")]
    public void AcceptAnyServerCertificate_ReturnsFalse_WhenTheChainIsInvalid()
    {
        var accepted = InsecureVendorBulletinClient.AcceptAnyServerCertificate(
            null,
            null,
            null,
            SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors);

        Assert.False(accepted);
    }
}
