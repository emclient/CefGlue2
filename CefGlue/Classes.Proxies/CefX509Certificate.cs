using System;
using Xilium.CefGlue.Interop;

namespace Xilium.CefGlue;

public sealed unsafe partial class CefX509Certificate
{
    /// <summary>
    /// Returns the number of certificates in the issuer chain.
    /// If 0, the certificate is self-signed.
    /// </summary>
    public long GetIssuerChainSize() => (long)IssuerChainSize;

    /// <summary>
    /// Returns the DER encoded data for the certificate issuer chain.
    /// If we failed to encode a certificate in the chain it is still
    /// present in the array but is an empty string.
    /// </summary>
    public void GetDerEncodedIssuerChain(out long chainCount, out CefBinaryValue chain)
    {
        UIntPtr n_chainCount;
        cef_binary_value_t* n_chain;

        cef_x509_certificate_t.get_derencoded_issuer_chain(_self, &n_chainCount, &n_chain);

        chainCount = (long)n_chainCount;
        chain = CefBinaryValue.FromNative(n_chain);
    }

    /// <summary>
    /// Returns the PEM encoded data for the certificate issuer chain.
    /// If we failed to encode a certificate in the chain it is still
    /// present in the array but is an empty string.
    /// </summary>
    public void GetPEMEncodedIssuerChain(out long chainCount, out CefBinaryValue chain)
    {
        UIntPtr n_chainCount;
        cef_binary_value_t* n_chain;

        cef_x509_certificate_t.get_pemencoded_issuer_chain(_self, &n_chainCount, &n_chain);

        chainCount = (long)n_chainCount;
        chain = CefBinaryValue.FromNative(n_chain);
    }
}
