using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.FlowLine.Mos;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.FlowLine
{
    public abstract partial class BaseFlow
    {
        //  todo  补全 FlowContext
        
        // 默认第一步，申请流程开始 
        public abstract Task<ResultMo<FlowInfo>> Apply();
        
        public abstract Task Entry();
        
        public abstract Task End();
   
    }

    public abstract class BaseDataFlow<TFlowEntity> : BaseFlow
    {
        public override Task<ResultMo<FlowInfo>> Apply()
        {
            //  需创建数据
           return Task.FromResult(new ResultMo<FlowInfo>());
        }

        //  获取流核心信息
        public abstract Task GetFlowInfo(TaskReq req);
    }

}
