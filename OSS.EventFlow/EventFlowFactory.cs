#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体辅助生成类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-01-24
*       
*****************************************************************************/

#endregion

using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    /// <summary>
    /// 流体生成器
    /// </summary>
    /// <typeparam name="InFlowContext"></typeparam>
    /// <typeparam name="OutFlowContext"></typeparam>
    public class EventFlowGenerator<InFlowContext, OutFlowContext>
        where InFlowContext : IFlowContext
        where OutFlowContext : IFlowContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startPipe"></param>
        public EventFlowGenerator(BasePipe<InFlowContext> startPipe)
        {
            StartPipe = startPipe;
        }

        /// <inheritdoc />
        public EventFlowGenerator(BasePipe<InFlowContext> startPipe, IPipeAppender<OutFlowContext> endPipeAppender):this(startPipe)
        {
            NPipeAppender = endPipeAppender;
        }

        internal BasePipe<InFlowContext> StartPipe { get; }
        internal IPipeAppender<OutFlowContext> NPipeAppender { get; set; }
    }


    /// <summary>
    /// EventFlow 创建工厂
    /// </summary>
    public static class EventFlowFactory
    {
        /// <summary>
        /// 由第一个节点开始创建流体
        /// </summary>
        /// <typeparam name="InFlowContext"></typeparam>
        /// <typeparam name="OutFlowContext"></typeparam>
        /// <param name="firstPipe"></param>
        /// <returns></returns>
        public static EventFlowGenerator<InFlowContext, OutFlowContext> Create<InFlowContext, OutFlowContext>(BaseSinglePipe<InFlowContext, OutFlowContext> firstPipe)
            where InFlowContext : IFlowContext
            where OutFlowContext : IFlowContext
        {
            return new EventFlowGenerator<InFlowContext, OutFlowContext>(firstPipe);
        }

        /// <summary>
        /// 由第一个节点开始创建流体
        /// </summary>
        /// <typeparam name="InFlowContext"></typeparam>
        /// <typeparam name="OutFlowContext"></typeparam>
        /// <typeparam name="NOutFlowContext"></typeparam>
        /// <param name="firstPipe"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static EventFlowGenerator<InFlowContext, NOutFlowContext> AsFlowAndAdd<InFlowContext, OutFlowContext,  NOutFlowContext>(this BaseSinglePipe<InFlowContext, OutFlowContext> firstPipe, 
            BaseSinglePipe<OutFlowContext, NOutFlowContext> nextPipe)
            where InFlowContext : IFlowContext
            where OutFlowContext : IFlowContext
            where NOutFlowContext : IFlowContext
        {
            firstPipe.Append(nextPipe);
            return new EventFlowGenerator<InFlowContext, NOutFlowContext>(firstPipe, nextPipe);
        }

        /// <summary>
        /// 添加新的管道
        /// </summary>
        /// <typeparam name="InFlowContext"></typeparam>
        /// <typeparam name="OutFlowContext"></typeparam>
        /// <typeparam name="NOutFlowContext"></typeparam>
        /// <param name="flowGenerator"></param>
        /// <param name="nextPipe"></param>
        /// <returns></returns>
        public static EventFlowGenerator<InFlowContext, NOutFlowContext> AddPipe<InFlowContext, OutFlowContext,  NOutFlowContext>(this EventFlowGenerator<InFlowContext, OutFlowContext> flowGenerator,
            BaseSinglePipe<OutFlowContext, NOutFlowContext> nextPipe)
            where InFlowContext : IFlowContext
            where OutFlowContext : IFlowContext
            where NOutFlowContext : IFlowContext
        {
            flowGenerator.NPipeAppender.Append(nextPipe);
            return new EventFlowGenerator<InFlowContext, NOutFlowContext>(flowGenerator.StartPipe, nextPipe);
        }

        /// <summary>
        ///  生成流体
        /// </summary>
        /// <returns></returns>
        public static EventFlow<InFlowContext, OutFlowContext> ToFlow<InFlowContext, OutFlowContext>(this EventFlowGenerator<InFlowContext, OutFlowContext> flowGenerator)
            where InFlowContext : IFlowContext
            where OutFlowContext : IFlowContext
        {
            return new EventFlow<InFlowContext, OutFlowContext>(flowGenerator.StartPipe, flowGenerator.NPipeAppender);
        }

    }
}
