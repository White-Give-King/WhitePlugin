using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Users;
using WhitePlugin.Serializable;
using WhitePlugin.Extensions;

namespace WhitePlugin.Commands;
[Command("back")]
[CommandDescription("Teleport back to where you died")]
[CommandActor(typeof(UnturnedUser))]
public class Back : Command
{
    private readonly IStringLocalizer _StringLocalizer;
    private readonly IUserManager _UserManager;
    private readonly WhitePlugin _WhitePlugin;
    private readonly IConfiguration _configuration;
    private readonly IUserDataStore _UserDataStore;
    private readonly ILogger<WhitePlugin> _Logger;

    public Back(
        IConfiguration configuration,
        IStringLocalizer stringLocalizer,
        IServiceProvider serviceProvider,
        ILogger<WhitePlugin> logger,
        IUserManager userManager,
        IUserDataStore userDataStore,
        WhitePlugin whitePlugin) : base(serviceProvider)
    {
        _configuration = configuration;
        _StringLocalizer = stringLocalizer;
        _UserManager = userManager;
        _WhitePlugin = whitePlugin;
        _UserDataStore = userDataStore;
        _Logger = logger;
    }

    protected override async UniTask OnExecuteAsync()
    {
        if (Context.Parameters.Length != 0) {
            throw new CommandWrongUsageException(Context);
        }

        UnturnedUser uPlayer = (UnturnedUser)Context.Actor;
        var userData = await _UserDataStore.GetUserDataAsync(uPlayer.Id, uPlayer.Type) ?? throw new UserFriendlyException(_StringLocalizer["back:none"]);
        var data = userData.Data ?? throw new UserFriendlyException(_StringLocalizer["back:none"]);

        if (!data.ContainsKey("deathLocation"))
        {
            throw new UserFriendlyException(_StringLocalizer["back:none"]);
        }

        var backLocation = Vector3.Deserialize(data["deathLocation"]) ?? throw new UserFriendlyException(_StringLocalizer["back:none"]);

        await uPlayer.Player.Player.TeleportToLocationAsync(backLocation.ToUnityVector3());
        await PrintAsync(_StringLocalizer["back:success"]);
    }
}