#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 流体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-02-09
*       
*****************************************************************************/

#endregion

using OSS.PipeLine.Mos;

namespace OSS.PipeLine.Interface
{
    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipe
    {
        /// <summary>
        ///  管道类型
        /// </summary>
        PipeType PipeType { get; }

        /// <summary>
        ///  管道编码
        /// </summary>
        public string PipeCode { get; set; }
    }

    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipeLine : IPipe
    {
        /// <summary>
        ///  开始管道
        /// </summary>
        public IPipe StartPipe { get; }
        /// <summary>
        ///  结束管道
        /// </summary>
        public IPipe EndPipe { get; }
    }

}
