using Microsoft.Extensions.Primitives;
using System.Security.Cryptography.Xml;
using Yarp.ReverseProxy.Configuration;

namespace YARPProgrammaticConfiguration
{
    public class YarpProxyConfig : IProxyConfig
    {
        readonly List<RouteConfig> _routes;
        readonly List<ClusterConfig> _clusters;
        readonly CancellationChangeToken _changeToken;
        readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public YarpProxyConfig()
        {
            _routes = GenerateRoutes();
            _clusters = GenerateClusters();
            _cts = new CancellationTokenSource();
            _changeToken = new CancellationChangeToken(_cts.Token);
        }
        public IReadOnlyList<RouteConfig> Routes => _routes;
        public IReadOnlyList<ClusterConfig> Clusters => _clusters;
        public IChangeToken ChangeToken => _changeToken;

        private List<ClusterConfig> GenerateClusters()
        {
            var collection = new List<ClusterConfig>();
            collection.Add(new ClusterConfig()
            {
                ClusterId = "FirstCluster",
                Destinations = new Dictionary<string, DestinationConfig>{
            {
                "server", new DestinationConfig()
                {
                    Address = "http://localhost:5167"
                }
            }
        }
            });
            return collection;
        }

        private List<RouteConfig> GenerateRoutes()
        {
            var collection = new List<RouteConfig>();
            collection.Add(new RouteConfig()
            {
                RouteId = "Route1",
                ClusterId = "FirstCluster",
                Match = new RouteMatch()
                {
                    Path = "/api/{**catch-all}"
                },
                //Transforms= 
            });
            return collection;
        }
    }
}
