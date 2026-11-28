# Contributing

Thanks for your interest in contributing to MetaTube! Please read the
[Code of Conduct](CODE_OF_CONDUCT.md) before taking part.

## Questions and Bug Reports

- Ask questions and share ideas in
  [Discussions](https://github.com/metatube-community/jellyfin-plugin-metatube/discussions).
- Check the [Wiki](https://metatube-community.github.io/wiki/) for setup and
  configuration help.
- Report bugs and request features with the
  [issue templates](https://github.com/metatube-community/jellyfin-plugin-metatube/issues/new/choose).
  Search existing issues first to avoid duplicates.
- Report security vulnerabilities privately as described in the
  [Security Policy](SECURITY.md).

## Development

You need the [.NET SDK](https://dotnet.microsoft.com/download) 10.x.

Build the Jellyfin plugin:

```bash
dotnet build --configuration Release
```

Build the Emby plugin:

```bash
dotnet build --configuration Release.Emby
```

Emby-specific code is guarded by the `__EMBY__` symbol, so please make sure both
builds pass before opening a pull request.

See [Build from source](https://metatube-community.github.io/wiki/build-from-source/)
for more details.

## Pull Requests

Keep each pull request focused on a single change, and follow the commit and
pull request conventions in [AGENTS.md](../AGENTS.md).
