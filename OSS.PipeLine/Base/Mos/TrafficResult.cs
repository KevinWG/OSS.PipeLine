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
    public readonly struct TrafficResult
    {
        public static TrafficResult Green { get; } =
            new TrafficResult(SignalFlag.Green_Pass, string.Empty, string.Empty);

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="tSignal"></param>
        /// <param name="blockedPipeCode"></param>
        public TrafficResult(TrafficSignal tSignal, string blockedPipeCode)
        {
            blocked_pipe_code = blockedPipeCode;

            this.signal = tSignal.signal;
            this.msg    = tSignal.msg;
        }

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public TrafficResult(SignalFlag signal, string blockedPipeCode, string msg)
        {
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
    }

    /// <summary>
    ///  流动结果（附带其他结果）
    /// </summary>
    public readonly struct TrafficResult<THandleResult, TOut>
    {

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficSignal"></param>
        /// <param name="nextParas"></param>
        /// <param name="blockedPipeCode"></param>
        public TrafficResult(TrafficSignal trafficSignal, string blockedPipeCode, TOut nextParas)
            : this(trafficSignal.signal, blockedPipeCode, trafficSignal.msg, nextParas, default)
        {
        }
        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficSignal"></param>
        /// <param name="nextParas"></param>
        /// <param name="blockedPipeCode"></param>
        public TrafficResult(TrafficSignal<THandleResult> trafficSignal, string blockedPipeCode, TOut nextParas)
            : this(trafficSignal.signal, blockedPipeCode, trafficSignal.msg, nextParas, trafficSignal.result)
        {
        }

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="nextParas"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public TrafficResult(SignalFlag signal, string blockedPipeCode, string msg, TOut nextParas):
            this(signal, blockedPipeCode, msg, nextParas,default)
        {
        }

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="nextParas"></param>
        /// <param name="activityResult"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public TrafficResult(SignalFlag signal, string blockedPipeCode, string msg, TOut nextParas,
            THandleResult activityResult)
        {
            next_paras        = nextParas;
            blocked_pipe_code = blockedPipeCode;
            result            = activityResult;

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
        ///  下节管道上下文参数
        /// </summary>
        public TOut next_paras { get; }

        /// <summary>
        ///  活动执行方法结果
        /// </summary>
        public THandleResult result { get; }
    }

    public static class TrafficResultMap
    {
        public static TrafficResult ToResult<TOut, TActivityResult>(this TrafficResult<TOut, TActivityResult> otRes)
        {
            return new TrafficResult(otRes.signal, otRes.blocked_pipe_code, otRes.msg);
        }
    }


}