using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DesktopApplication.Core;

/// <summary>
/// Downloads a vendor bulletin over HTTPS and requires a valid server certificate.
/// The process still uses the host TLS/Schannel stack for the handshake.
/// </summary>
public sealed class InsecureVendorBulletinClient
{
    public const string Marker = "TLS_CERTIFICATE_VALIDATED";
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
        return sslPolicyErrors == SslPolicyErrors.None;
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
