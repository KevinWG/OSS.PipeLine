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
    /// <summary>
    ///  活动基类
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseActivity<TContext> : BaseSinglePipe<TContext, TContext>
        where TContext : IPipeContext
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

        internal override async Task<bool> InterHandling(TContext context)
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
    ///  空上下文
    /// </summary>
    public class EmptyContext : IPipeContext
    {

    }

    /// <summary>
    /// 空活动
    /// </summary>
    public class EmptyActivity : BaseActivity<EmptyContext>
    {
        protected override Task<bool> Executing(EmptyContext data)
        {
            return Task.FromResult(true);
        }
    }


 

}
