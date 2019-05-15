using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Common.Plugs.LogPlug;
using OSS.EventNode.Executor;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// todo  重新激活处理
    /// </summary>
    public abstract partial class BaseNode<TTReq, TTRes>
    {
        #region 节点执行入口

        // 基类入口方法
        public Task<NodeResponse<TTRes>> Process(TTReq req)
        {
            var nodeResp = new NodeResponse<TTRes> { node_status = NodeStatus.WaitProcess };

            return  TryProcess(req, nodeResp);
        }


        private async Task<NodeResponse<TTRes>> TryProcess(TTReq req,NodeResponse<TTRes> nodeResp)
        {
            try
            {
                //  检查初始化
                var checkRes = await ProcessCheck(req, nodeResp);
                if (!checkRes)
                    return nodeResp;

                // 【2】 任务处理执行方法
                await Excuting(req, nodeResp);

                //  【3】 扩展后置执行方法
                await ProcessEnd(req, nodeResp);

                //  结束， 如果节点是暂停状态，需要保存上下文请求信息
                if (nodeResp.node_status == NodeStatus.ProcessPaused)
                    await TrySaveNodeContext(req, nodeResp);
                return nodeResp;
            }
            catch (Exception e)
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = new TTRes().WithResult(SysResultTypes.ApplicationError,
                    "Error occurred during Node [Process]!");

                LogUtil.Error($"sys_ret:{nodeResp.resp.sys_ret}, ret:{nodeResp.resp.ret},msg:{nodeResp.resp.msg}, Detail:{e}",
                    NodeMeta.node_id, ModuleName);
            }

            await TrySaveNodeContext(req, nodeResp);
            return nodeResp;
        }



        #endregion

        #region 生命周期扩展方法

        protected virtual Task<TTRes> ProcessPreCheck(TTReq req)
        {
            return Task.FromResult(new TTRes());
        }

        protected virtual Task ProcessEnd(TTReq req, NodeResponse<TTRes> resp)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法

        private async Task<bool> ProcessCheck(TTReq req, NodeResponse<TTRes> nodeResp)
        {
            if (string.IsNullOrEmpty(NodeMeta?.node_id))
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = new TTRes().WithResult(SysResultTypes.ApplicationError, ResultTypes.InnerError,
                    "Node id can't be null!");
                return false;
            }

            var res = await ProcessPreCheck(req);
            if (!res.IsSuccess())
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = res;
                return false;
            }

            return true;
        }


        internal virtual async Task Excuting(TTReq req, NodeResponse<TTRes> nodeResp)
        {
            // 获取任务元数据列表
            var tasks = await GetTasks();
            if (tasks == null || !tasks.Any())
                throw new ResultException(SysResultTypes.ApplicationError, ResultTypes.ObjectNull,
                    $"{this.GetType()} have no tasks can be Runed!");

            // 执行处理结果
            await ExcutingWithTasks(req, nodeResp, tasks);
        }

        #endregion

        #region 辅助方法 —— 节点内部任务执行
        
        private async Task ExcutingWithTasks(TTReq req, NodeResponse<TTRes> nodeResp, IList<IBaseTask<TTReq>> tasks)
        {
            if (NodeMeta.Process_type == NodeProcessType.Parallel)
            {
                await this.Excuting_Parallel(req, nodeResp, tasks);
            }
            else
            {
                await this.Excuting_Sequence(req, nodeResp, tasks);
            }
        }

        #endregion
    }
}
