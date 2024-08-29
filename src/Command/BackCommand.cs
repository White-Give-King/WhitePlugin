using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;

namespace WhitePlugin.Commands;
[Command("back")] // The primary name for the command. Usually, it is defined as lowercase. 
[CommandDescription("Send teleport request.")] // Description. Try to keep it short and simple.
[CommandActor(typeof(UnturnedUser))]
public class BackCommand : Command
{
    private readonly IStringLocalizer _StringLocalizer;
    private readonly IUserManager _UserManager;
    private readonly WhitePlugin _WhitePlugin;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WhitePlugin> _Logger;

    public BackCommand(
        IConfiguration configuration,
        IStringLocalizer stringLocalizer,
        IServiceProvider serviceProvider,
        ILogger<WhitePlugin> logger,
        IUserManager userManager,
        WhitePlugin whitePlugin) : base(serviceProvider)
    {
        _configuration = configuration;
        _StringLocalizer = stringLocalizer;
        _UserManager = userManager;
        _WhitePlugin = whitePlugin;
        _Logger = logger;
    }

    protected override async Task OnExecuteAsync()
    {
        ICommandActor player = Context.Actor;
        string playerId = Context.Actor.Id;
        
        string toPlayerName = await Context.Parameters.GetAsync<string>(0);
        IUser toPlayer = await _UserManager.FindUserAsync("", toPlayerName, UserSearchMode.FindByName) ?? throw new UserFriendlyException("未找到");
        string toPlayerId = toPlayer.Id;

        _Logger.LogDebug(playerId, toPlayerId);
    }
}