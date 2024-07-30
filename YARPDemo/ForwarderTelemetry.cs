using Yarp.ReverseProxy.Forwarder;

namespace YARPDemo
{
    public enum ForwarderStage : int
    {
        SendAsyncStart = 1,
        SendAsyncStop,
        RequestContentTransferStart,
        ResponseContentTransferStart,
        ResponseUpgrade,
    }
    public interface IForwarderTelemetryConsumer
    {
        /// <summary>
        /// Called before forwarding a request.
        /// </summary>
        /// <param name="timestamp">Timestamp when the event was fired.</param>
        /// <param name="destinationPrefix"></param>
        void OnForwarderStart(DateTime timestamp, string destinationPrefix) { }

        /// <summary>
        /// Called after forwarding a request.
        /// </summary>
        /// <param name="timestamp">Timestamp when the event was fired.</param>
        /// <param name="statusCode">The status code returned in the response.</param>
        void OnForwarderStop(DateTime timestamp, int statusCode) { }

        /// <summary>
        /// Called before <see cref="OnForwarderStop(DateTime, int)"/> if forwarding the request failed.
        /// </summary>
        /// <param name="timestamp">Timestamp when the event was fired.</param>
        /// <param name="error"><see cref="ForwarderError"/> information for the forwarding failure.</param>
        void OnForwarderFailed(DateTime timestamp, ForwarderError error) { }

        /// <summary>
        /// Called when reaching a given stage of forwarding a request.
        /// </summary>
        /// <param name="timestamp">Timestamp when the event was fired.</param>
        /// <param name="stage">Stage of the forwarding operation.</param>
        void OnForwarderStage(DateTime timestamp, ForwarderStage stage) { }

        /// <summary>
        /// Called periodically while a content transfer is active.
        /// </summary>
        /// <param name="timestamp">Timestamp when the event was fired.</param>
        /// <param name="isRequest">Indicates whether we are transferring the content from the client to the backend or vice-versa.</param>
        /// <param name="contentLength">Number of bytes transferred.</param>
        /// <param name="iops">Number of read/write pairs performed.</param>
        /// <param name="readTime">Time spent reading from the source.</param>
        /// <param name="writeTime">Time spent writing to the destination.</param>
        void OnContentTransferring(DateTime timestamp, bool isRequest, long contentLength, long iops, TimeSpan readTime, TimeSpan writeTime) { }

        /// <summary>
        /// Called after transferring the request or response content.
        /// </summary>
        /// <param name="timestamp">Timestamp when the event was fired.</param>
        /// <param name="isRequest">Indicates whether we transfered the content from the client to the backend or vice-versa.</param>
        /// <param name="contentLength">Number of bytes transferred.</param>
        /// <param name="iops">Number of read/write pairs performed.</param>
        /// <param name="readTime">Time spent reading from the source.</param>
        /// <param name="writeTime">Time spent writing to the destination.</param>
        /// <param name="firstReadTime">Time spent on the first read of the source.</param>
        void OnContentTransferred(DateTime timestamp, bool isRequest, long contentLength, long iops, TimeSpan readTime, TimeSpan writeTime, TimeSpan firstReadTime) { }

        /// <summary>
        /// Called before forwarding a request.
        /// </summary>
        /// <param name="timestamp">Timestamp when the event was fired.</param>
        /// <param name="clusterId">Cluster ID</param>
        /// <param name="routeId">Route ID</param>
        /// <param name="destinationId">Destination ID</param>
        void OnForwarderInvoke(DateTime timestamp, string clusterId, string routeId, string destinationId) { }
    }
    public class ForwarderTelemetry : IForwarderTelemetryConsumer
    {

        /// Called before forwarding a request.
        public void OnForwarderStart(DateTime timestamp, string destinationPrefix)
        {
            Console.WriteLine($"Forwarder Telemetry [{timestamp:HH:mm:ss.fff}] => OnForwarderStart :: Destination prefix: {destinationPrefix}");
        }

        /// Called after forwarding a request.
        public void OnForwarderStop(DateTime timestamp, int statusCode)
        {
            Console.WriteLine($"Forwarder Telemetry [{timestamp:HH:mm:ss.fff}] => OnForwarderStop :: Status: {statusCode}");
        }

        /// Called before <see cref="OnForwarderStop(DateTime, int)"/> if forwarding the request failed.
        public void OnForwarderFailed(DateTime timestamp, ForwarderError error)
        {
            Console.WriteLine($"Forwarder Telemetry [{timestamp:HH:mm:ss.fff}] => OnForwarderFailed :: Error: {error.ToString()}");
        }

        /// Called when reaching a given stage of forwarding a request.
        public void OnForwarderStage(DateTime timestamp, ForwarderStage stage)
        {
            Console.WriteLine($"Forwarder Telemetry [{timestamp:HH:mm:ss.fff}] => OnForwarderStage :: Stage: {stage.ToString()}");
        }

        /// Called periodically while a content transfer is active.
        public void OnContentTransferring(DateTime timestamp, bool isRequest, long contentLength, long iops, TimeSpan readTime, TimeSpan writeTime)
        {
            Console.WriteLine($"Forwarder Telemetry [{timestamp:HH:mm:ss.fff}] => OnContentTransferring :: Is request: {isRequest}, Content length: {contentLength}, IOps: {iops}, Read time: {readTime:s\\.fff}, Write time: {writeTime:s\\.fff}");
        }

        /// Called after transferring the request or response content.
        public void OnContentTransferred(DateTime timestamp, bool isRequest, long contentLength, long iops, TimeSpan readTime, TimeSpan writeTime, TimeSpan firstReadTime)
        {
            Console.WriteLine($"Forwarder Telemetry [{timestamp:HH:mm:ss.fff}] => OnContentTransferred :: Is request: {isRequest}, Content length: {contentLength}, IOps: {iops}, Read time: {readTime:s\\.fff}, Write time: {writeTime:s\\.fff}");
        }

        /// Called before forwarding a request from `ForwarderMiddleware`, therefore is not called for direct forwarding scenarios.
        public void OnForwarderInvoke(DateTime timestamp, string clusterId, string routeId, string destinationId)
        {
            var context = new HttpContextAccessor().HttpContext;
            var YarpFeature = context.GetReverseProxyFeature();

            var dests = from d in YarpFeature.AvailableDestinations
                        select d.Model.Config.Address;

            Console.WriteLine($"Destinations: {string.Join(", ", dests)}");
            Console.WriteLine($"Forwarder Telemetry [{timestamp:HH:mm:ss.fff}] => OnForwarderInvoke:: Cluster id: {clusterId}, Route Id: {routeId}, Destination: {destinationId}");
        }


    }
}
