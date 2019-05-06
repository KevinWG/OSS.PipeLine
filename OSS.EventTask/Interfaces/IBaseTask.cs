using OSS.Common.ComModels;
using OSS.EventTask.MetaMos;
using OSS.EventTask.Mos;

namespace OSS.EventTask.Interfaces
{
    public interface IBaseTask
    {
         InstanceType InstanceType { get;  }

        OwnerType OwnerType { get; set; }
    }

    public class BaseMetaTask<TTContext,TTRes> : BaseMetaProvider<TaskMeta, BaseMetaTask<TTContext, TTRes>>, IBaseTask
        where TTContext : TaskContext<TTRes>
        where TTRes : ResultMo, new()
    {
        private const string _moduleName = "OSS.EventTask";
        
        public BaseMetaTask(TaskMeta meta):base(meta)
        {
            ModuleName = _moduleName;
            OwnerType = OwnerType.Task;
        }


        public InstanceType InstanceType { get; protected set; }

        public OwnerType OwnerType { get; set; }
    }



}
