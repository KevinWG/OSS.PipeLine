using OSS.EventTask.Mos;

namespace OSS.EventTask.MetaMos
{
    public class GroupEventTaskMeta: BaseTaskMeta
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



    public static class GroupMetaExtension
    {
        public static void WithGroupMeta(this TaskMeta taskMeta, GroupEventTaskMeta nodeMeta)
        {
            taskMeta.owner_type = nodeMeta.owner_type;
            taskMeta.flow_id = nodeMeta.flow_id;
            taskMeta.group_id = nodeMeta.group_id;
        }
    }


}
