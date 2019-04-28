using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Flow.Mos;

namespace OSS.TaskFlow.Flow
{
    
    public abstract partial class BaseFlow<TFlowData> 
    {
        //public async Task<ResultIdMo> Apply( TFlowData req)
        //{
            
        //}

        public abstract Task<ResultMo> Enter(FlowReq req);

        public abstract Task End();
    }

}
