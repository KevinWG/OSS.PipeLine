using OSS.Pipeline.Tests.Flow;
using OSS.Pipeline.Tests.FlowItems;

namespace OSS.Pipeline.Tests
{
    public class BuyFlow : Pipeline<ApplyContext, Empty>
    {
        //
        public static ApplyActivity     ApplyActivity { get; } = new ApplyActivity();
        public static AutoAuditActivity AuditActivity { get; } = new AutoAuditActivity();

        public static PayActivity PayActivity { get; } = new PayActivity();
        public static PayGateway  PayGateway  { get; } = new PayGateway();

        public static StockConnector StockConnector { get; } = new StockConnector();
        public static StockActivity  StockActivity  { get; } = new StockActivity();

        public static PayEmailConnector EmailConnector { get; } = new PayEmailConnector();
        public static SendEmailActivity EmailActivity  { get; } = new SendEmailActivity();

        private static EndGateway _endNode = new EndGateway();

        //  构造函数内定义流体关联
        public BuyFlow() : base("BUY_Flow", ApplyActivity, _endNode,new PipeLineOption()
        {
            Watcher = new FlowWatcher()
        })
        {
            ApplyActivity
                .Append(AuditActivity)

                .Append(PayActivity)
                .Append(PayGateway);

            // 网关分支 - 发送邮件分支
            PayGateway
                .Append(EmailConnector)
                .Append(EmailActivity)
                .Append(_endNode);

            // 网关分支- 入库分支
            PayGateway
                .Append(StockConnector)
                .Append(StockActivity)
                .Append(_endNode);
        }






    }
}
