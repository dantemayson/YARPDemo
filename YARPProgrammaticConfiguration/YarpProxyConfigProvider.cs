using Yarp.ReverseProxy.Configuration;

namespace YARPProgrammaticConfiguration
{
    public class YarpProxyConfigProvider : IProxyConfigProvider
    {
        public IProxyConfig GetConfig()
        {
            return new YarpProxyConfig();
        }
    }
}
