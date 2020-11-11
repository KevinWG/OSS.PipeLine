using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
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
