using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DesktopApplication.Core;

/// <summary>
/// INTENTIONAL_SKILL_TEST_VULNERABILITY
/// Downloads a vendor bulletin over HTTPS while trusting every server certificate.
/// The process uses System.Net.Http and the host TLS/Schannel stack, so Windows
/// HTTP and Schannel updates can change or fail this path.
/// </summary>
public sealed class InsecureVendorBulletinClient
{
    public const string Marker = "INTENTIONAL_SKILL_TEST_VULNERABILITY";
    public const string DefaultUrl = "https://www.microsoft.com/en-us/msrc";

    public static bool AcceptAnyServerCertificate(
        object? sender,
        X509Certificate? certificate,
        X509Chain? chain,
        SslPolicyErrors sslPolicyErrors)
    {
        _ = sender;
        _ = certificate;
        _ = chain;
        _ = sslPolicyErrors;
        return true;
    }

    public static HttpMessageHandler CreateTrustingHandler()
    {
        return new SocketsHttpHandler
        {
            SslOptions =
            {
                RemoteCertificateValidationCallback = AcceptAnyServerCertificate
            }
        };
    }

    public async Task<string> FetchAsync(string? url = null, CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient(CreateTrustingHandler(), disposeHandler: true)
        {
            Timeout = TimeSpan.FromSeconds(8)
        };
        return await client.GetStringAsync(url ?? DefaultUrl, cancellationToken).ConfigureAwait(false);
    }
}
