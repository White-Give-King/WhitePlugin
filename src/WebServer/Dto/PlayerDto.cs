using System;

namespace WhitePlugin.WebServer.Dto;

public class PlayerDto
{
    public string SteamName { get; set; }
    public string SteamId { get; set; }
    public string PlayerName { get; set; }
    public string Id { get; set; }
    public float Balance { get; set; }
    public string AvatarUrl { get; set; }
    public PlayerDto(
        string _SteamName = "SteamPlayer",
        string _SteamId = "0",
        string _playerName = "Player",
        string _Id  = "0",
        float _Balance = 0.0f,
        string _AvatarUrl = "https://cdn.discordapp.com/embed/avatars/0.png")
    {
        SteamName = _SteamName;
        SteamId = _SteamId;
        PlayerName = _playerName;
        Id = _Id;
        Balance = _Balance;
        AvatarUrl = _AvatarUrl;
    }
}
