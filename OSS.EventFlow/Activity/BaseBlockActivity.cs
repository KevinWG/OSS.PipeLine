#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  异步阻塞活动基类
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
    public abstract class BaseBlockActivity<TContext> : BaseActivity<TContext>,IBlockTunnel<TContext>
        where TContext : FlowContext
    {
        public abstract Task Push(TContext data);


        public async Task Pop(TContext data)
        {
            await Execute(data);
            await ToNextThrough(data);
        }
        

        internal override Task Through(TContext context)
        {
            return Push(context);
        }
    }
}
