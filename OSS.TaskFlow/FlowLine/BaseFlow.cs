using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Node.MetaMos;

namespace OSS.TaskFlow.FlowLine
{
    
    public abstract partial class BaseFlow<TFlowEntity> 
    {
        //public override Task<ResultMo<FlowInfo>> Apply()
        //{
        //    //  需创建数据
        //   return Task.FromResult(new ResultMo<FlowInfo>());
        //}
        public abstract Task<ResultMo<TFlowEntity>> Apply();

        public abstract Task<ResultMo> Entry(ExcuteReq req);

        public abstract Task End();


        //internal virtual Task<List<NodeMeta>> GetNextNodes(FlowContext context);


        //  获取流核心数据信息
        //public abstract Task GetFlowInfo(TaskReq<> req);
    }

}
