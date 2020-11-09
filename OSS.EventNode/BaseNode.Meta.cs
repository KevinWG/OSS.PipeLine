using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.EventNode.Interfaces;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventNode
{

    /// <summary>
    ///  节点运行时元数据信息
    /// </summary>
    public abstract partial class BaseNode<TTData, TTRes> 
        : BaseMeta<NodeMeta>, IEventNode<TTData, TTRes>
        where TTData : class 
        where TTRes : class, new()
    {

        protected BaseNode() : this(null)
        {
        }

        protected BaseNode(NodeMeta meta) : base(meta)
        {
        }

        #region 内部基础方法

        protected abstract Task<List<IEventTask<TTData,TTRes>>> GetTasks();

        #endregion

        #region 扩展方法

        /// <summary>
        ///  保存对应运行请求和重试相关信息
        /// </summary>
        /// <returns></returns>
        protected virtual Task SaveNodekContext(TTData data, TTRes resp, 
            RunCondition cond, IDictionary<TaskMeta, TaskResp<TTRes>> taskResults)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///  保存对应运行请求和重试相关信息
        /// </summary>
        /// <returns></returns>
        protected virtual Task SaveErrorNodeContext(TTData data, TTRes resp,
            RunCondition cond, IDictionary<TaskMeta, TaskResp<TTRes>> taskResults)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 辅助方法

        private Task TrySaveNodeContext(TTData data, NodeResp<TTRes> nodeResp)
        {
            //try
            //{
                var blockTaskResp = nodeResp[nodeResp.block_taskid];

                return nodeResp.node_status == NodeStatus.ProcessPaused
                    ? SaveNodekContext(data, nodeResp.resp, blockTaskResp.task_cond, nodeResp.TaskResults)
                    : SaveErrorNodeContext(data, nodeResp.resp, blockTaskResp.task_cond, nodeResp.TaskResults);
            //}
            //catch (Exception e)
            //{
            //    //  防止Provider中SaveTaskContext内部使用Task实现时，级联异常死循环
            //    LogHelper.Error($"Errors occurred during [Task context] saving. Detail:{e}", NodeMeta.node_id,
            //        EventTaskProvider.ModuleName);
            //}

            //return Task.CompletedTask;
        }

        #endregion
    }





}
