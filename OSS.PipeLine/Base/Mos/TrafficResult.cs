﻿#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

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
    public readonly struct TrafficResult<TRes, TOut>
    {
        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficSignal"></param>
        /// <param name="nextParas"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="res"></param>
        public TrafficResult(TrafficSignal trafficSignal, string blockedPipeCode, TRes res, TOut nextParas)
            : this(trafficSignal.signal, blockedPipeCode, trafficSignal.msg, res, nextParas)
        {
        }
        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="trafficSignal"></param>
        /// <param name="nextParas"></param>
        /// <param name="blockedPipeCode"></param>
        public TrafficResult(TrafficSignal<TRes> trafficSignal, string blockedPipeCode, TOut nextParas)
            : this(trafficSignal.signal, blockedPipeCode, trafficSignal.msg, trafficSignal.result, nextParas)
        {
        }

        /// <summary>
        /// 流动结果
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="nextParas"></param>
        /// <param name="executeResult"></param>
        /// <param name="blockedPipeCode"></param>
        /// <param name="msg"></param>
        public TrafficResult(SignalFlag signal, string blockedPipeCode, string msg, TRes executeResult, TOut nextParas)
        {
            output_paras        = nextParas;
            blocked_pipe_code = blockedPipeCode;
            result            = executeResult;

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
        public TOut output_paras { get; }

        /// <summary>
        ///  活动执行方法结果
        /// </summary>
        public TRes result { get; }
    }

    public static class TrafficResultMap
    {
        public static TrafficResult ToResult<TOut, TActivityResult>(this TrafficResult<TOut, TActivityResult> otRes)
        {
            return new TrafficResult(otRes.signal, otRes.blocked_pipe_code, otRes.msg);
        }
    }


}