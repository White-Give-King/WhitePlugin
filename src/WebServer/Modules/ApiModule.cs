using System;
using System.Text;
using System.Threading.Tasks;
using EmbedIO.WebApi;
using EmbedIO.WebSockets;

namespace WhitePlugin.WebServer.Modules;

public class ApiModule : WebApiModuleBase
{
    public ApiModule(string baseRoute, IServiceProvider serviceProvider) : base(baseRoute)
    {

    }

}