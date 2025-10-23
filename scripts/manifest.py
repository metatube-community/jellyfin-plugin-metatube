#!/usr/bin/env python3
import hashlib
import json
import os
import sys
import xml.etree.ElementTree as ET
from datetime import datetime
from urllib.request import urlopen
from packaging.version import Version


def md5sum(filename) -> str:
    with open(filename, 'rb') as f:
        return hashlib.md5(f.read()).hexdigest()


def get_jellyfin_version(csproj: str) -> str:
    tree = ET.parse(csproj)
    root = tree.getroot()

    for pkg in root.iter("PackageReference"):
        if pkg.attrib.get("Include") in ("Jellyfin.Controller", "Jellyfin.Model"):
            return Version(pkg.attrib.get("Version")).base_version

    raise Exception("Jellyfin version not found")


def generate(filename, version, csproj) -> dict:
    return {
        'checksum': md5sum(filename),
        'changelog': 'Auto Released by Actions',
        'targetAbi': f'{get_jellyfin_version(csproj)}.0',
        'sourceUrl': 'https://github.com/metatube-community/jellyfin-plugin-metatube/releases/download/'
                     f'v{version}/Jellyfin.MetaTube@v{version}.zip',
        'timestamp': datetime.now().strftime('%Y-%m-%dT%H:%M:%SZ'),
        'version': version
    }


def main() -> None:
    filename = sys.argv[1]
    version = filename.split('@', maxsplit=1)[1] \
        .removeprefix('v') \
        .removesuffix('.zip')

    csproj = os.path.join(os.path.dirname(__file__),
                          "../Jellyfin.Plugin.MetaTube/Jellyfin.Plugin.MetaTube.csproj")

    with urlopen(
            'https://raw.githubusercontent.com/metatube-community/jellyfin-plugin-metatube/dist/manifest.json') as f:
        manifest = json.load(f)

    manifest[0]['versions'].insert(0, generate(filename, version, csproj))

    with open('manifest.json', 'w') as f:
        json.dump(manifest, f, indent=2)


if __name__ == '__main__':
    main()
