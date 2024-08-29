using System;
using EmbedIO.WebApi;

namespace WhitePlugin.WebServer.Controllers;

public class BaseController : WebApiController
{
    public IServiceProvider ServiceProvider { get; }

    public BaseController(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }
}