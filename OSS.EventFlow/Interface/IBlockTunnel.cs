#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 流体异步阻塞通信管道部分
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion


using System.Threading.Tasks;

namespace OSS.EventFlow.Interface
{
    public interface IBlockTunnel<in TData>
    {
        Task Push(TData data);

        Task Pop(TData data);
    }
}
