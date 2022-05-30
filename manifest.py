import hashlib
import json
import sys
from datetime import datetime
from urllib.request import urlopen


def md5sum(filename):
    with open(filename, 'rb') as f:
        return hashlib.md5(f.read()).hexdigest()


def generate(filename, version):
    return {
        'checksum': md5sum(filename),
        'changelog': 'Auto Released by Actions',
        'targetAbi': '10.8.0.0',
        'sourceUrl': 'https://github.com/javtube/jellyfin-plugin-javtube/releases/download/'
                     f'v{version}/Jellyfin.JavTube@v{version}.zip',
        'timestamp': datetime.now().strftime('%Y-%m-%dT%H:%M:%SZ'),
        'version': version
    }


def main():
    filename = sys.argv[1]
    version = filename.split('@', maxsplit=1)[1] \
        .removeprefix('v') \
        .removesuffix('.zip')

    with urlopen('https://raw.githubusercontent.com/javtube/jellyfin-plugin-javtube/dist/manifest.json') as f:
        manifest = json.load(f)

    manifest[0]['versions'].insert(
        0,
        generate(filename, version)
    )

    with open('manifest.json', 'w') as f:
        json.dump(manifest, f, indent=2)


if __name__ == '__main__':
    main()