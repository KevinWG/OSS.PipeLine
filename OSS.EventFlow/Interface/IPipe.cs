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

using OSS.EventFlow.Mos;

namespace OSS.EventFlow.Interface
{
    /// <summary>
    ///  管道基础接口
    /// </summary>
    public interface IPipe
    {
        /// <summary>
        ///  管道类型
        /// </summary>
        PipeType pipe_type { get; }

        /// <summary>
        ///  管道编码
        /// </summary>
        public string pipe_code { get; set; }
    }
}
