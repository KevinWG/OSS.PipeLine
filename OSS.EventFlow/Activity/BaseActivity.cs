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
using OSS.EventFlow.Activity.Interface;
using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Activity
{
    /// <summary>
    ///  活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseActivity<TContext> : BaseSinglePipe<TContext, TContext>
        where TContext : IFlowContext
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        protected BaseActivity() : base(PipeType.Activity)
        {
        }

        /// <summary>
        ///  执行
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract Task<bool> Executing(TContext data);

        internal override async Task<bool> Through(TContext context)
        {
            var eRes = await Executing(context);
            if (eRes)
            {
                await ToNextThrough(context);
            }

            return eRes;
        }
    }


    /// <summary>
    /// 活动基类 的默认实现
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class DefaultActivity<TContext> : BaseActivity<TContext>
        where TContext : IFlowContext
    {
        private readonly IActivityProvider<TContext> _provider;

        /// <inheritdoc />
        public DefaultActivity(IActivityProvider<TContext> provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        protected override Task<bool> Executing(TContext data)
        {
            return _provider.Executing(data);
        }
    }

}
