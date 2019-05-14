namespace OSS.TaskFlow.Tests.TestOrder.AddOrderNode.Reqs
{
    // 添加订单请求实体
    public class AddOrderReq
    {
        public string source_ids { get; set; }
        public string title { get; set; }
        public decimal order_price { get; set; }
    }
}
