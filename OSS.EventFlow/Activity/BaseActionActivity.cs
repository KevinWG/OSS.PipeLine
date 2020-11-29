#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  外部动作活动
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    public abstract class BaseActionActivity<TContext,TResult> : BaseSinglePipe<TContext, TContext>
        where TContext : FlowContext
    {
        protected BaseActionActivity() : base(PipeType.Activity)
        {
        }

        protected abstract Task<TResult> ActionExecuting(TContext data);

        public async Task<TResult> Action(TContext data)
        {
            var res = await ActionExecuting(data);
            await ToNextThrough(data);
            return res;
        }

        public abstract Task Push(TContext data);

        internal override Task Through(TContext context)
        {
            return Push(context);
        }

      
    }
}
