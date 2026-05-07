# Caddy Reverse Proxy

## Purpose

This document provides a generic Caddy example for hosting the `bobweb` ASP.NET Core sample behind TLS. Keep host
names, email addresses, and other deployment-specific values out of the repo when adapting it for a real environment.

## Example

Replace `api.example.com` with your real API host name and point Caddy at the web sample running on port `8081`.

```caddyfile
api.example.com {
    encode gzip zstd

    reverse_proxy 127.0.0.1:8081
}
```

## Notes

- `bobweb` already processes forwarded protocol headers, which is the important proxy integration for HTTPS
  termination.
- The Bob deployment model uses a separate API hostname such as `api.bobcalypso.com`. `bobstatic/js/bob_config.js`
  can default to that API origin for hosted environments while keeping a localhost override for development.
- Treat this file as a public sample. Do not add real domain names, private IP addresses, or environment-specific
  operational details here.
