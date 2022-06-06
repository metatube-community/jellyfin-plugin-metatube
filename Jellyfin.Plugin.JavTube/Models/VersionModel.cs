using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class VersionModel
{
    [JsonPropertyName("version")] public string Version { get; set; }

    [JsonPropertyName("git_commit")] public string GitCommit { get; set; }
}