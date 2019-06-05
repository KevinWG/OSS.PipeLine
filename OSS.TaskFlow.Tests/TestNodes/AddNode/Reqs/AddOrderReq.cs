using OSS.EventTask.Mos;

namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs
{
    // 添加订单请求实体
    public class AddOrderReq
    {
        public string title { get; set; }
        
        public string source_ids { get; set; }
        public string coupon_id { get; set; }
    }

 
}
