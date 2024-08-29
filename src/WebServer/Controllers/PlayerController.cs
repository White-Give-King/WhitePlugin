using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Extensions.Economy.Abstractions;
using SDG.Unturned;
using WhitePlugin.WebServer.Dto;
using WhitePlugin.WebServer.Services;

namespace WhitePlugin.WebServer.Controllers;

public class PlayerController : BaseController
{
    private readonly IPermissionRoleStore _RoleStore;

    private readonly PlayerService _PlayerService;

    public PlayerController(IServiceProvider serviceProvider, IPermissionRoleStore roleStore, PlayerService playerService) : base(serviceProvider)
    {
        _RoleStore = roleStore;
        _PlayerService = playerService;
    }

    [Route(HttpVerbs.Get, "/players", false)]
    public async Task<ICollection<PlayerDto>> GetPlayers() => (await _PlayerService.GetPlayers())
        ?? throw HttpException.NotFound();

}