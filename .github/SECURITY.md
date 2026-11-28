# Security Policy

## Supported Versions

Only the latest release of the plugin receives security fixes. Please update to
the [latest release](https://github.com/metatube-community/jellyfin-plugin-metatube/releases/latest)
before reporting an issue.

The plugin supports stable versions of Jellyfin and Emby only, as listed in the
[README](https://github.com/metatube-community/jellyfin-plugin-metatube/blob/main/README.md#platforms).

## Reporting a Vulnerability

**Please do not report security vulnerabilities through public GitHub issues,
discussions, or pull requests.**

Instead, report them privately through
[GitHub Security Advisories](https://github.com/metatube-community/jellyfin-plugin-metatube/security/advisories/new).

Please include as much of the following as you can:

- The type of issue, e.g., credential leak, path traversal, or SSRF
- The affected plugin version and Jellyfin/Emby version
- Steps to reproduce the issue, or a proof of concept
- The impact of the issue and how an attacker might exploit it

We will acknowledge your report as soon as we can, keep you updated on the fix,
and credit you in the advisory unless you prefer to stay anonymous.

## Scope

This policy covers the code in this repository. Issues in the MetaTube server
should be reported to the
[metatube-sdk-go](https://github.com/metatube-community/metatube-sdk-go)
repository, and issues in Jellyfin or Emby themselves to their own projects.
