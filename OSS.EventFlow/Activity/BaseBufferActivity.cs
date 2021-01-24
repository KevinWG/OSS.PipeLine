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
using OSS.EventFlow.Activity.Interface;
using OSS.EventFlow.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    /// <summary>
    /// 异步消息延缓活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseBufferActivity<TContext> : BaseActivity<TContext>, IBufferTunnel<TContext>
        where TContext : IFlowContext
    {
        /// <inheritdoc />
        public abstract Task<bool> Push(TContext data);


        /// <inheritdoc />
        public async Task Pop(TContext data)
        {
            var res = await Executing(data);
            if (res)
            {
                await ToNextThrough(data);
                return;
            }

            await Block(data);
        }

        internal override Task<bool> Through(TContext context)
        {
            return Push(context);
        }
    }

    /// <summary>
    /// 默认异步消息延缓活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultBufferActivity<TContext> : BaseBufferActivity<TContext>
        where TContext : IFlowContext
    {

        private readonly IBufferActivityProvider<TContext> _provider;
        /// <summary>
        /// 异步消息延缓活动基类
        /// </summary>
        /// <param name="provider"></param>
        public DefaultBufferActivity(IBufferActivityProvider<TContext> provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        public override Task<bool> Push(TContext data)
        {
            return _provider.Push(data);
        }

        /// <inheritdoc />
        protected override Task<bool> Executing(TContext data)
        {
            return _provider.Executing(data);
        }
    }
}
