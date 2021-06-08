#region Copyright (C) 2021 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow - 被动委托活动接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2021-6-1
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;

namespace OSS.Pipeline.Interface
{

    /// <typeparam name="TFuncPara"></typeparam>
    /// <typeparam name="TFuncResult"></typeparam>
    public interface IActivity<in TFuncPara, TFuncResult> : IOutPipeAppender<TFuncResult>
    {
        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> Execute(TFuncPara context);
    }
}
