#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 流体基础管道部分
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{
    /// <summary>
    /// 管道基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BasePipe<TContext>
        where TContext : FlowContext
    {
        /// <summary>
        ///  管道类型
        /// </summary>
        public PipeType pipe_type { get; internal set; }

        /// <summary>
        ///  管道元数据信息
        /// </summary>
        public PipeMeta pipe_meta { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BasePipe(PipeType pipeType)
        {
            pipe_type = pipeType;
        }

        /// <summary>
        ///  管道通过方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract Task Through(TContext context);
    }


    /// <summary>
    ///  基础管道
    /// </summary>
    /// <typeparam name="InContext"></typeparam>
    /// <typeparam name="OutContext"></typeparam>
    public abstract class BaseSinglePipe<InContext, OutContext> : BasePipe<InContext>, INextPipeAppender<OutContext>
        where InContext : FlowContext
        where OutContext : FlowContext
    {
        internal BasePipe<OutContext> NextPipe { get; set; }

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="pipeType"></param>
        protected BaseSinglePipe(PipeType pipeType) : base(pipeType)
        {
        }

        /// <summary>
        ///  添加下个管道
        /// </summary>
        /// <param name="nextPipe"></param>
        internal virtual void InterAppend(BasePipe<OutContext> nextPipe)
        {
            NextPipe = nextPipe;
        }

        /// <summary>
        ///  添加下个管道
        /// </summary>
        /// <param name="nextPipe"></param>
        public void Append(BasePipe<OutContext> nextPipe)
        {
            InterAppend(nextPipe);
        }
    }
}
