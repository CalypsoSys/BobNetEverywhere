# Caddy Reverse Proxy

## Purpose

This document provides a generic Caddy example for hosting the `bobweb` ASP.NET Core sample behind TLS. Keep host
names, email addresses, and other deployment-specific values out of the repo when adapting it for a real environment.

## Example

Replace `bob.example.com` with your real host name and point Caddy at the web sample running on port `5080`.

```caddyfile
bob.example.com {
    encode gzip zstd

    reverse_proxy 127.0.0.1:5080
}
```

## Notes

- `bobweb` already processes forwarded protocol headers, which is the important proxy integration for HTTPS
  termination.
- The static sample uses same-origin `/api/*` calls by default in hosted environments. If you host the static files
  elsewhere, set `window.bobApiBaseUrl` in `bobstatic/js/bob_config.js` to the API origin used by the browser.
- Treat this file as a public sample. Do not add real domain names, private IP addresses, or environment-specific
  operational details here.
