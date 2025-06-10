namespace Xilium.CefGlue;

public sealed unsafe partial class CefX509CertPrincipal
{
    /// <summary>
    /// Returns a name that can be used to represent the issuer. It tries in this
    /// order: Common Name (CN), Organization Name (O) and Organizational Unit
    /// Name (OU) and returns the first non-empty one found.
    /// </summary>
    public string GetDisplayName() => DisplayName;

    /// <summary>
    /// Returns the common name.
    /// </summary>
    public string GetCommonName() => CommonName;

    /// <summary>
    /// Returns the locality name.
    /// </summary>
    public string GetLocalityName() => LocalityName;

    /// <summary>
    /// Returns the state or province name.
    /// </summary>
    public string GetStateOrProvinceName() => StateOrProvinceName;

    /// <summary>
    /// Returns the country name.
    /// </summary>
    public string GetCountryName() => CountryName;
}
