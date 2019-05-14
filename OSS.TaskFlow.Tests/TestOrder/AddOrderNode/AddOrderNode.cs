using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.EventNode;
using OSS.EventTask.Interfaces;
using OSS.EventTask.Mos;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs;
using OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Tasks;

namespace OSS.TaskFlow.Tests.TestOrder.Nodes
{
    /// <summary>
    ///   添加订单节点
    /// </summary>
    public class AddOrderNode : BaseStandNode<AddOrderReq, ResultIdMo>
    {
        private static List<IBaseTask<ExcuteReq<AddOrderReq>>>list; // = new List<IBaseTask<ExcuteReq<AddOrderReq>>>(){ new AddOrderTask() };

        public AddOrderNode()
        {
            list = new List<IBaseTask<ExcuteReq<AddOrderReq>>>() {new InsertOrderTask()};
        }
        
        protected override async Task<IList<IBaseTask<ExcuteReq<AddOrderReq>>>> GetTasks()
        {
            return list;
        }
    }
}
