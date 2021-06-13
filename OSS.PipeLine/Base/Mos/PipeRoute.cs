#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  管道路由
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;

namespace OSS.Pipeline
{
    /// <summary>
    ///  管道路由信息
    /// </summary>
    public class PipeRoute
    {
        /// <summary>
        ///  管道类型
        /// </summary>
        public PipeType pipe_type { get; set; }

        /// <summary>
        ///  管道编码
        /// </summary>
        public string pipe_code { get; set; }

        /// <summary>
        ///  下一个节点
        /// </summary>
        public PipeRoute next { get; set; }

        /// <summary>
        ///  分支网关的所有子分支节点( PipeType.BranchGateway 
        /// </summary>
        public IList<PipeRoute> nexts { get; set; }

        /// <summary>
        ///  流水线下内部子管道( PipeType.Flow 
        /// </summary>
        public PipeRoute inter_pipe { get; set; }
    }
}
