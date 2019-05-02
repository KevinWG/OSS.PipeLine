using OSS.Common.ComModels;
using OSS.EventTask.Mos;

namespace OSS.EventTask
{
    public abstract partial class BaseStandTask<TReq, TRes> : BaseTask<TaskContext<TReq>,TRes>
        where TRes : ResultMo, new()
    {
       
        
    }
}