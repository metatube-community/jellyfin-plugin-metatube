<h1 align="center">Jellyfin Plugin MetaTube</h1>
<p align="center"><b><a href="README.md">English</a> | 日本語</b></p>

<p align="center">
<img alt="Plugin Banner" src="https://metatube-community.github.io/images/banner-dark.png"/>
<br/>
<br/>

<a href="https://github.com/metatube-community/jellyfin-plugin-metatube/actions">
<img alt="GitHub Workflow Status" src="https://img.shields.io/github/actions/workflow/status/metatube-community/jellyfin-plugin-metatube/dotnetcore.yml?branch=main&logo=githubactions">
</a>
<img alt="GitHub top language" src="https://img.shields.io/github/languages/top/metatube-community/jellyfin-plugin-metatube?color=%23239120&label=.NET&logo=dotnet">
<a href="https://github.com/metatube-community/jellyfin-plugin-metatube/blob/main/LICENSE">
<img alt="License" src="https://img.shields.io/github/license/metatube-community/jellyfin-plugin-metatube?logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyBmaWxsPSJ3aGl0ZXNtb2tlIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgdmlld0JveD0iMCAwIDE2IDE2Ij48cGF0aCBkPSJNOC43NS43NVYyaC45ODVjLjMwNCAwIC42MDMuMDguODY3LjIzMWwxLjI5LjczNmMuMDM4LjAyMi4wOC4wMzMuMTI0LjAzM2gyLjIzNGEuNzUuNzUgMCAwIDEgMCAxLjVoLS40MjdsMi4xMTEgNC42OTJhLjc1Ljc1IDAgMCAxLS4xNTQuODM4bC0uNTMtLjUzLjUyOS41MzEtLjAwMS4wMDItLjAwMi4wMDItLjAwNi4wMDYtLjAwNi4wMDUtLjAxLjAxLS4wNDUuMDRjLS4yMS4xNzYtLjQ0MS4zMjctLjY4Ni40NUMxNC41NTYgMTAuNzggMTMuODggMTEgMTMgMTFhNC40OTggNC40OTggMCAwIDEtMi4wMjMtLjQ1NCAzLjU0NCAzLjU0NCAwIDAgMS0uNjg2LS40NWwtLjA0NS0uMDQtLjAxNi0uMDE1LS4wMDYtLjAwNi0uMDA0LS4wMDR2LS4wMDFhLjc1Ljc1IDAgMCAxLS4xNTQtLjgzOEwxMi4xNzggNC41aC0uMTYyYy0uMzA1IDAtLjYwNC0uMDc5LS44NjgtLjIzMWwtMS4yOS0uNzM2YS4yNDUuMjQ1IDAgMCAwLS4xMjQtLjAzM0g4Ljc1VjEzaDIuNWEuNzUuNzUgMCAwIDEgMCAxLjVoLTYuNWEuNzUuNzUgMCAwIDEgMC0xLjVoMi41VjMuNWgtLjk4NGEuMjQ1LjI0NSAwIDAgMC0uMTI0LjAzM2wtMS4yODkuNzM3Yy0uMjY1LjE1LS41NjQuMjMtLjg2OS4yM2gtLjE2MmwyLjExMiA0LjY5MmEuNzUuNzUgMCAwIDEtLjE1NC44MzhsLS41My0uNTMuNTI5LjUzMS0uMDAxLjAwMi0uMDAyLjAwMi0uMDA2LjAwNi0uMDE2LjAxNS0uMDQ1LjA0Yy0uMjEuMTc2LS40NDEuMzI3LS42ODYuNDVDNC41NTYgMTAuNzggMy44OCAxMSAzIDExYTQuNDk4IDQuNDk4IDAgMCAxLTIuMDIzLS40NTQgMy41NDQgMy41NDQgMCAwIDEtLjY4Ni0uNDVsLS4wNDUtLjA0LS4wMTYtLjAxNS0uMDA2LS4wMDYtLjAwNC0uMDA0di0uMDAxYS43NS43NSAwIDAgMS0uMTU0LS44MzhMMi4xNzggNC41SDEuNzVhLjc1Ljc1IDAgMCAxIDAtMS41aDIuMjM0YS4yNDkuMjQ5IDAgMCAwIC4xMjUtLjAzM2wxLjI4OC0uNzM3Yy4yNjUtLjE1LjU2NC0uMjMuODY5LS4yM2guOTg0Vi43NWEuNzUuNzUgMCAwIDEgMS41IDBabTIuOTQ1IDguNDc3Yy4yODUuMTM1LjcxOC4yNzMgMS4zMDUuMjczczEuMDItLjEzOCAxLjMwNS0uMjczTDEzIDYuMzI3Wm0tMTAgMGMuMjg1LjEzNS43MTguMjczIDEuMzA1LjI3M3MxLjAyLS4xMzggMS4zMDUtLjI3M0wzIDYuMzI3WiIvPjwvc3ZnPg%3D%3D">
</a>
<a href="https://star-history.com/#metatube-community/jellyfin-plugin-metatube&Date">
<img alt="GitHub Stars" src="https://img.shields.io/github/stars/metatube-community/jellyfin-plugin-metatube?style=flat&logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyBmaWxsPSJ3aGl0ZXNtb2tlIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgdmlld0JveD0iMCAwIDE2IDE2Ij48cGF0aCBkPSJNOCAuMjVhLjc1Ljc1IDAgMCAxIC42NzMuNDE4bDEuODgyIDMuODE1IDQuMjEuNjEyYS43NS43NSAwIDAgMSAuNDE2IDEuMjc5bC0zLjA0NiAyLjk3LjcxOSA0LjE5MmEuNzUxLjc1MSAwIDAgMS0xLjA4OC43OTFMOCAxMi4zNDdsLTMuNzY2IDEuOThhLjc1Ljc1IDAgMCAxLTEuMDg4LS43OWwuNzItNC4xOTRMLjgxOCA2LjM3NGEuNzUuNzUgMCAwIDEgLjQxNi0xLjI4bDQuMjEtLjYxMUw3LjMyNy42NjhBLjc1Ljc1IDAgMCAxIDggLjI1Wm0wIDIuNDQ1TDYuNjE1IDUuNWEuNzUuNzUgMCAwIDEtLjU2NC40MWwtMy4wOTcuNDUgMi4yNCAyLjE4NGEuNzUuNzUgMCAwIDEgLjIxNi42NjRsLS41MjggMy4wODQgMi43NjktMS40NTZhLjc1Ljc1IDAgMCAxIC42OTggMGwyLjc3IDEuNDU2LS41My0zLjA4NGEuNzUuNzUgMCAwIDEgLjIxNi0uNjY0bDIuMjQtMi4xODMtMy4wOTYtLjQ1YS43NS43NSAwIDAgMS0uNTY0LS40MUw4IDIuNjk0WiIvPjwvc3ZnPg%3D%3D">
</a>
<a href="https://github.com/metatube-community/jellyfin-plugin-metatube/releases">
<img alt="Downloads" src="https://img.shields.io/github/downloads/metatube-community/jellyfin-plugin-metatube/total?logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPHN2ZyBmaWxsPSJ3aGl0ZXNtb2tlIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiIgdmlld0JveD0iMCAwIDE2IDE2Ij48cGF0aCBkPSJNMi43NSAxNEExLjc1IDEuNzUgMCAwIDEgMSAxMi4yNXYtMi41YS43NS43NSAwIDAgMSAxLjUgMHYyLjVjMCAuMTM4LjExMi4yNS4yNS4yNWgxMC41YS4yNS4yNSAwIDAgMCAuMjUtLjI1di0yLjVhLjc1Ljc1IDAgMCAxIDEuNSAwdjIuNUExLjc1IDEuNzUgMCAwIDEgMTMuMjUgMTRaIi8%2BPHBhdGggZD0iTTcuMjUgNy42ODlWMmEuNzUuNzUgMCAwIDEgMS41IDB2NS42ODlsMS45Ny0xLjk2OWEuNzQ5Ljc0OSAwIDEgMSAxLjA2IDEuMDZsLTMuMjUgMy4yNWEuNzQ5Ljc0OSAwIDAgMS0xLjA2IDBMNC4yMiA2Ljc4YS43NDkuNzQ5IDAgMSAxIDEuMDYtMS4wNmwxLjk3IDEuOTY5WiIvPjwvc3ZnPg%3D%3D">
</a>
<a href="https://github.com/metatube-community/jellyfin-plugin-metatube/releases">
<img alt="Releases" src="https://img.shields.io/github/v/release/metatube-community/jellyfin-plugin-metatube?include_prereleases&logo=semver">
</a>
</p>

## 概要

Jellyfin／Emby 向けに開発された、とても便利なメタデータプラグインです。

## 特徴

- 完全なデータ：タイトル、概要、出演者、タグ、評価 などを含む豊富な情報を提供。
- 強力な検索機能：多数のスクレイピングソースから作品や俳優情報を検索可能。
- トレーラー機能：動画をダウンロードせずに オンラインで予告編を視聴。
- スケジュールタスク：自動的に作品タグを整理し、バックグラウンドでプラグインを更新。
- 顔認識機能：内蔵の顔認識により、顔を中心にポスター画像を自動トリミング。
- 自動翻訳：特定のメタデータ内容を必要な言語に翻訳可能。

## 対応プラットフォーム

[![Jellyfin](https://img.shields.io/static/v1?color=%2300A4DC&style=for-the-badge&label=Jellyfin&logo=jellyfin&message=12.x)](https://jellyfin.org/)
[![Emby](https://img.shields.io/static/v1?color=%2352B54B&style=for-the-badge&label=Emby&logo=emby&message=4.9.x)](https://emby.media/)

_※本プロジェクトは Jellyfin／Emby の安定版のみをサポートしています。_

## ドキュメント

- [プラグインのインストール](https://metatube-community.github.io/wiki/plugin-installation/)
- [バックエンドのデプロイ](https://metatube-community.github.io/wiki/server-deployment/)
- [命名規則](https://metatube-community.github.io/wiki/naming-rules/)
- [自動翻訳](https://metatube-community.github.io/wiki/auto-translation/)
- [ソースからのビルド](https://metatube-community.github.io/wiki/build-from-source/)
- [データソース](https://metatube-community.github.io/wiki/metadata-providers/)

詳細な使い方や解説は [Wiki](https://metatube-community.github.io/wiki/) をご参照ください。

## コミュニティ

質問や提案などは、[Discussions](https://github.com/metatube-community/jellyfin-plugin-metatube/discussions) にてお気軽にどうぞ。

## ライセンス

本プラグインは [MIT](https://github.com/metatube-community/jellyfin-plugin-metatube/blob/main/LICENSE) ライセンスの下で公開されています。

## スター履歴

[![Star History Chart](https://api.star-history.com/svg?repos=metatube-community/jellyfin-plugin-metatube&type=Date)](https://star-history.com/#metatube-community/jellyfin-plugin-metatube&Date)
