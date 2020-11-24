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
using OSS.EventFlow.Mos;

namespace OSS.EventFlow
{

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

    public abstract class BaseSinglePipe<InContext, OutContext> : BasePipe<InContext>
        where InContext : FlowContext
        where OutContext : FlowContext
    {
        internal BasePipe<OutContext> NextPipe { get; set; }

        protected BaseSinglePipe(PipeType pipeType) : base(pipeType)
        {
        }
        
        /// <summary>
        ///  添加下个管道
        /// </summary>
        /// <param name="nextPipe"></param>
        public void Append(BasePipe<OutContext> nextPipe)
        {
            NextPipe = nextPipe;
        }
        
     }
    
    //public abstract class BaseSinglePipe<ContextType> : BaseSinglePipe<ContextType,ContextType>
    //    where ContextType : FlowContext
    //{
    //    protected BaseSinglePipe(PipeType pipeType) : base(pipeType)
    //    {
    //    }

    //    internal override Task Through(ContextType context)
    //    {
    //        return NextPipe.Through(context);
    //    }
    //}
}
