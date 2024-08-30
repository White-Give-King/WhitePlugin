
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;

namespace WhitePlugin.Commands;
[Command("tpaccept")] // The primary name for the command. Usually, it is defined as lowercase. 
[CommandAlias("tpac")]
[CommandDescription("Accept teleport request.")] // Description. Try to keep it short and simple.
[CommandActor(typeof(UnturnedUser))]
public class TPAccept : Command
{
    private readonly IStringLocalizer _StringLocalizer;
    private readonly IUserManager _UserManager;
    private readonly WhitePlugin _WhitePlugin;
    private readonly IConfiguration _configuration;

    public TPAccept(
        IConfiguration configuration,
        IStringLocalizer stringLocalizer,
        IServiceProvider serviceProvider,
        IUserManager userManager,
        WhitePlugin whitePlugin) : base(serviceProvider)
    {
        _configuration = configuration;
        _StringLocalizer = stringLocalizer;
        _UserManager = userManager;
        _WhitePlugin = whitePlugin;
    }

    protected override async Task OnExecuteAsync()
    {
        foreach (string playerId in _WhitePlugin.TPARequests.Keys)
        {
            if (_WhitePlugin.TPARequests[playerId].Item1 == "") {
                
            }
        }
    }
}