#region Copyright (C) 2020 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventFlow -  空上下文
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2020-11-22
*       
*****************************************************************************/

#endregion

namespace OSS.Pipeline
{
    /// <summary>
    ///  空值
    /// </summary>
    public struct Empty
    {
        /// <summary>
        ///  默认空上下文
        /// </summary>
        public static Empty Default { get; } 

        static Empty() {
            Default = new Empty();
        }
    }
}