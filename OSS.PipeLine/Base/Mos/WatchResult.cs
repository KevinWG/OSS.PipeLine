namespace OSS.Pipeline
{
    /// <summary>
    ///  流动结果
    /// </summary>
    public readonly struct WatchResult
    {
        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public WatchResult(SignalFlag signal, string blockedPipeCode, string msg)
            : this(signal, default, blockedPipeCode, msg)
        {
        }


        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="activityResult"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public WatchResult(SignalFlag signal, object activityResult, string blockedPipeCode, string msg)
        {
            activity_result     = activityResult;
            blocked_pipe_code = blockedPipeCode;

            this.signal = signal;
            this.msg    = msg;
        }


        /// <summary>
        ///  流动信号
        /// </summary>
        public SignalFlag signal { get; }

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
        public object activity_result { get; }
    }
    internal static class WatchResultMap
    {
        public static WatchResult ToWatchResult<TOut, TActivityResult>(this TrafficResult<TOut, TActivityResult> tRes)
        {

            return new WatchResult(tRes.signal, tRes.result, tRes.blocked_pipe_code, tRes.msg);
        }

    }
}