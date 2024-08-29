using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMod.API.Users;
using OpenMod.Extensions.Economy.Abstractions;
using SDG.Unturned;
using WhitePlugin.WebServer.Dto;

namespace WhitePlugin.WebServer.Services;

public class PlayerService
{
    private readonly IUserManager _userManager;
    private readonly IEconomyProvider _economyProvider;

    public PlayerService(IServiceProvider serviceProvider, IUserManager userManager, IEconomyProvider economyProvider)
    {
        _userManager = userManager;
        _economyProvider = economyProvider;
    }

    public async Task<ICollection<PlayerDto>> GetPlayers()
    {
        var players = new List<PlayerDto>();

        foreach (SteamPlayer user in PlayerTool.getSteamPlayers())
        {
            players.Add(new PlayerDto
            {
                SteamName = user.playerID.nickName.ToString(),
                SteamId = user.playerID.steamID.m_SteamID.ToString(),
                PlayerName = user.playerID.playerName.ToString(),
                Id = user.playerID.characterID.ToString(),
                Balance = Convert.ToSingle(await _economyProvider.GetBalanceAsync(
                    user.playerID.characterID.ToString(),
                    "")),
            });
        }

        return players;
    }

}