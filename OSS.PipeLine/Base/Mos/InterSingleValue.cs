

namespace OSS.Pipeline
{
    internal struct InterSingleValue
    {
        public InterSingleValue(TrafficSignal trafficSignal,string blockedPipeCode)
        {
            traffic_signal    = trafficSignal;
            blocked_pipe_code = blockedPipeCode;
        }

        public TrafficSignal traffic_signal { get; }

        public string blocked_pipe_code { get; }
    }
}
