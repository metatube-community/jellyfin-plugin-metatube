#!/usr/bin/env python3
"""Check a locally built release without fetching or publishing a manifest."""

import argparse
from pathlib import Path
from zipfile import ZipFile

from manifest import generate


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--configuration", choices=("Release", "Release.Emby"), required=True)
    parser.add_argument("--version", required=True)
    args = parser.parse_args()

    project = Path(__file__).resolve().parent.parent / "Jellyfin.Plugin.MetaTube"
    is_emby = args.configuration == "Release.Emby"
    platform = "Emby" if is_emby else "Jellyfin"
    framework = "net8.0" if is_emby else "net10.0"
    archive = project / "bin" / f"{platform}.MetaTube@v{args.version}.zip"
    assembly = project / "bin" / args.configuration / framework / "MetaTube.dll"

    with ZipFile(archive) as release:
        assert release.namelist() == ["MetaTube.dll"], "Release must contain only MetaTube.dll"
        assert release.read("MetaTube.dll") == assembly.read_bytes(), "Packaged DLL differs from build"

    if not is_emby:
        entry = generate(str(archive), args.version, str(project / "Jellyfin.Plugin.MetaTube.csproj"))
        assert entry["targetAbi"] == "12.0.0.0", "Jellyfin 12 requires a matching manifest ABI"
        assert entry["version"] == args.version
        assert entry["sourceUrl"].endswith(archive.name)
        assert len(entry["checksum"]) == 32

    print(f"Validated {platform}: {framework}, release archive" + (" and ABI 12.0.0.0" if not is_emby else ""))


if __name__ == "__main__":
    main()
