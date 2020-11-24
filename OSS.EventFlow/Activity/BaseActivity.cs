#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  活动基类
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
    public abstract class BaseActivity<TContext> : BaseSinglePipe<TContext,TContext>
        where TContext : FlowContext
    {
        protected BaseActivity() : base(PipeType.Activity)
        {
        }
        
        protected abstract Task Execute(TContext data);
        
        internal override async Task Through(TContext context)
        {
            await Execute(context);
            await NextPipe.Through(context);
        }
    }


}
