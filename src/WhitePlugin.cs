using System;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using OpenMod.API.Permissions;
using EmbedIO;
using EmbedIO.Security;
using WhitePlugin.WebServer.Modules;
using OpenMod.Core.Helpers;
using System.Threading.Tasks;

[assembly: PluginMetadata("WhitePlugin", DisplayName = "White Plugin")]
namespace WhitePlugin;
public class WhitePlugin : OpenModUnturnedPlugin
{
    private readonly IConfiguration _Configuration;
    private readonly IStringLocalizer _StringLocalizer;
    private readonly ILogger<WhitePlugin> _Logger;
    private readonly IPermissionRegistry _PermissionRegistry;
    private readonly IServiceProvider _ServiceProvider;

    public Dictionary<string, (string, DateTime)> TPARequests;

    public WhitePlugin(
        IConfiguration configuration,
        IStringLocalizer stringLocalizer,
        ILogger<WhitePlugin> logger,
        IPermissionRegistry permissionRegistry,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _Configuration = configuration;
        _StringLocalizer = stringLocalizer;
        _Logger = logger;
        _PermissionRegistry = permissionRegistry;
        _ServiceProvider = serviceProvider;
        TPARequests = new Dictionary<string, (string, DateTime)>();
    }

    protected override async UniTask OnLoadAsync()
    {
        RegisterPermissions();
        
        TPARequests = new Dictionary<string, (string, DateTime)>();

        AsyncHelper.Schedule("Processing Request", () => HttpServerThread());
        _Logger.LogInformation("Http server started.");

        await base.OnLoadAsync();
    }

    private void RegisterPermissions()
    {
        _PermissionRegistry.RegisterPermission(this, "command.TPA", "Use TPA command.");
        _Logger.LogInformation("Permissions registered.");
    }

    private async Task HttpServerThread()
    {
        var url = "http://localhost:8888/";
        var server = new EmbedIO.WebServer(
            o => o.WithUrlPrefix(url)
            .WithMode(HttpListenerMode.EmbedIO))
            .WithIPBanning(
                o => o.WithMaxRequestsPerSecond()
                .WithRegexRules("HTTP exception 404"))
            .WithLocalSessionManager()
            .WithModule(new ApiModule("/api", _ServiceProvider));

        // Listen for state changes.
        server.StateChanged += (s, e) => _Logger.LogInformation($"WebServer New State - {e.NewState}");

        await server.RunAsync();
    }
}
