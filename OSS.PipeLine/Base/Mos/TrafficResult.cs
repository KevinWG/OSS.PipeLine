#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  通行流动结果
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-06-06
*       
*****************************************************************************/

#endregion

namespace OSS.Pipeline
{
    /// <summary>
    ///  流动结果
    /// </summary>
    public class TrafficResult:TrafficSignal
    {
        /// <summary>
        ///  通行结果
        /// </summary>
        public static TrafficResult GreenResult { get; } =
            new TrafficResult(SignalFlag.Green_Pass, string.Empty, string.Empty);

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="tSignal"></param>
        /// <param name="blockedPipeCode"></param>
        public TrafficResult(TrafficSignal tSignal, string blockedPipeCode):this(tSignal.signal, blockedPipeCode, tSignal.msg)
        {
        }

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public TrafficResult(SignalFlag signal, string blockedPipeCode, string msg):base(signal,  msg)
        {
            blocked_pipe_code = blockedPipeCode;
        }
        
        /// <summary>
        /// 阻塞的管道PipeCode
        /// </summary>
        public string blocked_pipe_code { get; }
    }

    /// <summary>
    ///  流动结果（附带其他结果）
    /// </summary>
    public class TrafficResult<TRes, TOut>: TrafficResult
    {   /// <summary>
        /// 流动结果
        /// </summary>
        public TrafficResult(TrafficSignal<TRes, TOut> trafficSignal, string blockedPipeCode)
            : this(trafficSignal, blockedPipeCode, trafficSignal.output)
        {
        }

        /// <summary>
        /// 流动结果
        /// </summary>
        public TrafficResult(TrafficSignal<TRes> trafficSignal, string blockedPipeCode, TOut nextParas)
            : this(trafficSignal, blockedPipeCode, trafficSignal.result, nextParas)
        {
        }

        /// <summary>
        /// 流动结果
        /// </summary>
        public TrafficResult(TrafficSignal trafficSignal, string blockedPipeCode, TRes res, TOut nextParas)
            : this(trafficSignal.signal, blockedPipeCode, trafficSignal.msg, res, nextParas)
        {
        }
        
        /// <summary>
        /// 流动结果
        /// </summary>
        public TrafficResult(SignalFlag signal, string blockedPipeCode, string msg, TRes executeResult, TOut nextParas)
            : base(signal, blockedPipeCode, msg)
        {
            output = nextParas;
            result = executeResult;
        }

        /// <summary>
        ///  下节管道上下文参数
        /// </summary>
        public TOut output { get; }

        /// <summary>
        ///  活动执行方法结果
        /// </summary>
        public TRes result { get; }
    }
}