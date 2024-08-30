using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Commands;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Core.Helpers;
using OpenMod.Unturned.Users;

namespace WhitePlugin.Commands;
[Command("tpa")] // The primary name for the command. Usually, it is defined as lowercase. 
[CommandSyntax("<player>")] // Describe the syntax/usage. Use <> for required arguments and [] for optional arguments.
[CommandDescription("Send teleport request.")] // Description. Try to keep it short and simple.
[CommandActor(typeof(UnturnedUser))]
public class TPA : Command
{
    private readonly IStringLocalizer _StringLocalizer;
    private readonly IUserManager _UserManager;
    private readonly WhitePlugin _WhitePlugin;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WhitePlugin> _Logger;

    public TPA(
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
        // Not enabled TPA
        if (!_configuration.GetSection("TPA:enable").Get<bool>())
        {
            throw new UserFriendlyException(_StringLocalizer["commands:TPA:not_enabled"]);
        }
        // Check player permission
        if (await CheckPermissionAsync("TPA") != PermissionGrantResult.Grant)
        {
            throw new NotEnoughPermissionException(Context, "command.TPA");
        }

        string toPlayerName = await Context.Parameters.GetAsync<string>(0);
        IUser toPlayer = await _UserManager.FindUserAsync("", toPlayerName, UserSearchMode.FindByName) ?? throw new UserFriendlyException(_StringLocalizer["commands:TPA:cant_find"]);
        string toPlayerId = toPlayer.Id;
        
        ICommandActor player = Context.Actor;
        string playerId = Context.Actor.Id;

        int coolDown = _configuration.GetSection("TPA:cool_down_seconds").Get<int>();
        DateTime expiryDate = DateTime.Now.AddSeconds(coolDown);

        if (_WhitePlugin.TPARequests.ContainsKey(playerId))
        {
            // A request has been sent
            if (_WhitePlugin.TPARequests[playerId].Item1 == toPlayerId)
            {
                throw new UserFriendlyException(_StringLocalizer["commands:TPA:pending"]);
            }
            // Change request player
            else
            {
                _WhitePlugin.TPARequests[playerId] = (toPlayerId, expiryDate);
            }
        }
        // Send a new request
        else
        {
            _WhitePlugin.TPARequests.Add(playerId, (toPlayerId, expiryDate));
        }

        AsyncHelper.Schedule("Request Delete Thread", () => RequestDeleteThread(_WhitePlugin, player, toPlayer, coolDown));
        await player.PrintMessageAsync(_StringLocalizer["commands:TPA:success", new { Player = toPlayerName }]);
    }

    public async Task RequestDeleteThread(WhitePlugin whitePlugin, ICommandActor player, IUser toPlayer, int coolDown)
    {
        await Task.Delay(TimeSpan.FromSeconds(coolDown));
        if (whitePlugin.TPARequests.ContainsKey(player.Id))
        {
            if (whitePlugin.TPARequests[player.Id].Item1 == toPlayer.Id)
            {
                whitePlugin.TPARequests.Remove(player.Id);
            }
        }
    }
}
