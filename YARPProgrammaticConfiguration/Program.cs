using Yarp.ReverseProxy.Configuration;
using YARPProgrammaticConfiguration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IProxyConfigProvider, YarpProxyConfigProvider>();
builder.Services.AddReverseProxy();

var app = builder.Build();

app.MapReverseProxy(x =>
{
    x.UseSessionAffinity();
    x.UseLoadBalancing();
});

app.MapGet("/", () => "Hello World!");
app.Run();
