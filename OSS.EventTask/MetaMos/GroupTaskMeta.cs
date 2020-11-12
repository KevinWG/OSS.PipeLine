#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 群组事件任务配置元数据
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2019-04-07
*       
*****************************************************************************/

#endregion


using OSS.EventTask.Mos;

namespace OSS.EventTask.MetaMos
{
    public class GroupEventTaskMeta:BaseTaskMeta 
    {
        public string group_alias { get; set; }

        public GroupProcessType Process_type { get; set; }

        public OwnerType owner_type { get; set; }
    }

    public enum GroupProcessType
    {
        /// <summary>
        ///  串行
        /// </summary>
        Serial,

        /// <summary>
        /// 并行
        /// </summary>
        Parallel
    }
}
