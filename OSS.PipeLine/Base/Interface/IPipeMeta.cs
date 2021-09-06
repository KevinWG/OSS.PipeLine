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

namespace OSS.Pipeline.Interface
{
    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipeMeta
    {
        /// <summary>
        ///  管道类型
        /// </summary>
        PipeType PipeType { get; }

        /// <summary>
        ///  管道编码
        /// </summary>
        string PipeCode { get; set; }
    }
}
