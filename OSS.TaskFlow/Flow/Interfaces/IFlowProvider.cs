using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.TaskFlow.Flow.Mos;

namespace OSS.TaskFlow.Flow.Interfaces
{
    public interface IFlowProvider<TDomain>
    {
        /// <summary>
        ///  生成运行Id
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<ResultIdMo> GenerateRunId(FlowContext context);
    }
}
