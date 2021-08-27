using OSS.Pipeline.Tests.Flow;
using OSS.Pipeline.Tests.FlowItems;

namespace OSS.Pipeline.Tests
{
    public class BuyFlow : Pipeline<ApplyContext, Empty>
    {

        private static readonly ApplyActivity _startNode = new ApplyActivity();
        private static readonly EndGateway    _endNode   = new EndGateway();
        
        public ApplyActivity ApplyActivity
        {
            get { return _startNode; }
        }

        public AutoAuditActivity AuditActivity { get; } = new AutoAuditActivity();

        public PayActivity PayActivity { get; } = new PayActivity();
        public PayGateway  PayGateway  { get; } = new PayGateway();

        public StockConnector StockConnector { get; } = new StockConnector();
        public StockActivity  StockActivity  { get; } = new StockActivity();

        public PayEmailConnector EmailConnector { get; } = new PayEmailConnector();
        public SendEmailActivity EmailActivity  { get; } = new SendEmailActivity();
        
        //  构造函数内定义流体关联
        public BuyFlow() : base("Buy_Flow", _startNode, _endNode, new PipeLineOption()  {Watcher = new FlowWatcher()})
        {
        }

        protected override void InitialPipes()
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
