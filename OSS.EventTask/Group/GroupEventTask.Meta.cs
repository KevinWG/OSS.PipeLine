using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventTask.Group.MetaMos;
using OSS.EventTask.Group.Mos;
using OSS.EventTask.Interfaces;

namespace OSS.EventTask.Group
{
    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class GroupEventTask<TTData, TTRes>
        : BaseEventTask<GroupEventTaskMeta, TTData, GroupTaskResp<TTRes>>
        where TTData : class 
        where TTRes : class, new()
    {

        protected GroupEventTask() 
        {
        }

        protected GroupEventTask(GroupEventTaskMeta meta) : base(meta)
        {
        }

        #region 内部基础方法

        protected abstract Task<List<IEventTask<TTData,TTRes>>> GetTasks();

        #endregion

    }





}
