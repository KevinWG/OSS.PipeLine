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
        /// <summary>
        /// 通行
        /// </summary>
        public static TrafficResult GreenResult { get; } =
            new TrafficResult(SignalFlag.Green_Pass, null, string.Empty, string.Empty);

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficValue"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="funcResult"></param>
        public TrafficResult(TrafficSignal trafficValue, string blockedPipeCode, object funcResult)
            : this(trafficValue.signal, funcResult, blockedPipeCode, trafficValue.msg)
        {

        }

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficSignal"></param>
        /// <param name="funcResult"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public TrafficResult(SignalFlag trafficSignal, object funcResult, string blockedPipeCode, string msg)
        {
            signal            = trafficSignal;
            func_result       = funcResult;
            blocked_pipe_code = blockedPipeCode;

            this.msg = msg;
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
        public object func_result { get; }
    }
}