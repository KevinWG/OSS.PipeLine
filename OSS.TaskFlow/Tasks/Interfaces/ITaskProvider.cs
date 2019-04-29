using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Tasks.Mos;

namespace OSS.TaskFlow.Tasks.Interfaces
{
    public interface ITaskProvider
    {
        /// <summary>
        ///  生成运行Id
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
       Task<ResultIdMo> GenerateRunId(TaskContext context);
    }


}
