

namespace OSS.Pipeline
{
    /// <summary>
    ///  流动信号
    /// </summary>
    public readonly struct TrafficSingleValue
    {
        /// <summary>
        /// 流动信号
        /// </summary>
        /// <param name="trafficSignal"></param>
        /// <param name="trafficMsg"></param>
        public TrafficSingleValue(TrafficSignal trafficSignal, string trafficMsg=null)
        {
            signal = trafficSignal;
            msg    = trafficMsg;
        }

        /// <summary>
        ///  信号
        /// </summary>
        public TrafficSignal signal { get; }

        /// <summary>
        ///  消息
        /// </summary>
        public string msg { get; }
    }


    /// <summary>
    ///  流动结果
    /// </summary>
    public readonly struct TrafficResult
    {
        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficValue"></param>
        /// <param name="blockedPipeCode"></param>
        public TrafficResult(TrafficSingleValue trafficValue, string blockedPipeCode = null)
        :this(trafficValue.signal,null,blockedPipeCode,trafficValue.msg)
        {
        }


        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficSignal"></param>
        /// <param name="funcResult"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public TrafficResult(TrafficSignal trafficSignal, object funcResult=null, string blockedPipeCode = null, string msg = null)
        {
            signal            = trafficSignal;
            func_result       = funcResult;
            blocked_pipe_code = blockedPipeCode;

            this.msg          = msg;
        }


        /// <summary>
        ///  流动信号
        /// </summary>
        public TrafficSignal signal { get; }

        /// <summary>
        /// 阻塞的管道PipeCode
        /// </summary>
        public string blocked_pipe_code { get; }

        /// <summary>
        ///  消息
        /// </summary>
        public string msg { get; }
        /// <summary>
        ///  
        /// </summary>
        public object func_result { get; }
    }
}
