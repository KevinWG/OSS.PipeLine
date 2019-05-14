using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.EventNode.Executor;
using OSS.EventNode.MetaMos;
using OSS.EventNode.Mos;
using OSS.EventTask.Interfaces;

namespace OSS.EventNode
{
    /// <summary>
    ///  基础工作节点
    /// todo  重新激活处理
    /// todo  全部节点回退
    /// todo  保存未激活信息和节点列表
    /// </summary>
    public abstract partial class BaseNode<TTReq, TTRes>
    {
        #region 节点执行入口

        // 重写基类入口方法
        public async Task<NodeResponse<TTRes>> Process(TTReq req)
        {
            var nodeResp = new NodeResponse<TTRes> {node_status = NodeStatus.WaitProcess};

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
                return nodeResp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        #endregion

        #region 生命周期扩展方法

        protected virtual Task<ResultMo> ProcessPreCheck(TTReq req)
        {
            return Task.FromResult(new ResultMo());
        }

        protected virtual Task ProcessEnd(TTReq req, NodeResponse<TTRes> resp)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region 内部扩展方法

        private async Task<bool> ProcessCheck(TTReq req, NodeResponse<TTRes> nodeResp)
        {
            var checkRes = ProcessCheckInternal(req);
            if (!checkRes.IsSuccess())
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = checkRes;
                return false;
            }

            var res = await ProcessPreCheck(req);
            if (!res.IsSuccess())
            {
                nodeResp.node_status = NodeStatus.ProcessFailed;
                nodeResp.resp = checkRes;
                return false;
            }

            return true;
        }

        //  检查context内容
        internal virtual TTRes ProcessCheckInternal(TTReq context)
        {
            if (string.IsNullOrEmpty(NodeMeta?.node_id))
            {
                return new TTRes().WithResult(SysResultTypes.ApplicationError, ResultTypes.InnerError,
                    "Node id can't be null!");
            }

            //if (string.IsNullOrEmpty(context..exe_id))
            //    context.exe_id = DateTime.Now.Ticks.ToString();

            return new TTRes();
        }



        #endregion

        #region 辅助方法 —— 节点内部任务执行

        private async Task Excuting(TTReq req, NodeResponse<TTRes> nodeResp)
        {
            // 获取任务元数据列表
            var tasks = await GetTasks();
            if (tasks == null || !tasks.Any())
                throw new ResultException(SysResultTypes.ApplicationError, ResultTypes.ObjectNull,
                    $"{this.GetType()} have no tasks can be Runed!");

            // 执行处理结果
            await ExcutingWithTasks(req, nodeResp, tasks);
        }

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
