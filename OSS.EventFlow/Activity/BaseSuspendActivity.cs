#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  异步消息延缓活动基类
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

namespace OSS.EventFlow.Activity
{
    /// <summary>
    /// 异步消息延缓活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBlockActivity<TContext> : BaseActivity<TContext>, ISuspendTunnel<TContext>
        where TContext : FlowContext
    {
        /// <inheritdoc />
        public abstract Task<bool> Suspend(TContext data);


        /// <inheritdoc />
        public async Task Resume(TContext data)
        {
            var res = await Execute(data);
            if (res)
            {
                await ToNextThrough(data);
                return;
            }

            await Block(data);
        }

        internal override Task<bool> Through(TContext context)
        {
            return Suspend(context);
        }
    }
}
