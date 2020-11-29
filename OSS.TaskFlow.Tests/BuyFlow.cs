using System.Threading.Tasks;
using OSS.EventFlow;
using OSS.EventFlow.Activity;
using OSS.EventFlow.Connector;
using OSS.TaskFlow.Tests.FlowContexts;
using OSS.Tools.Log;

namespace OSS.TaskFlow.Tests
{
    //  申请,  审核，  支付， 入库  
    public class BuyFlow : BaseFlow<ApplyContext, StockContext>
    {
        public readonly ApplyActivity ApplyActivity = new ApplyActivity();
        public readonly AutoAuditActivity AutoAuditActivity = new AutoAuditActivity();

        public readonly PayConnector PayConnector = new PayConnector();
        public readonly PayActivity PayActivity = new PayActivity();

        public readonly StockConnector StockConnector = new StockConnector();
        public readonly StockActivity StockActivity = new StockActivity();

        public BuyFlow()
        {
            Start(ApplyActivity);
            End(StockActivity);

            ApplyActivity.Append(AutoAuditActivity).Append(PayConnector).Append(PayActivity).Append(StockConnector).Append(StockActivity);
        }

    }


    public class ApplyActivity : BaseActivity<ApplyContext>
    {
        protected override Task Execute(ApplyContext data)
        {
            LogHelper.Info("这里刚才发生了一个采购申请"); 
            return Task.CompletedTask;
        }
    }


    public class AutoAuditActivity : BaseActivity<ApplyContext>
    {
        protected override Task Execute(ApplyContext data)
        {
            LogHelper.Info("管理员审核通过");
            return Task.CompletedTask;
        }
    }


    public class PayConnector:BaseConnector<ApplyContext, PayContext>
    {
        protected override PayContext Convert(ApplyContext inContextData)
        {
           return new PayContext(){id = inContextData.id};
        }
    }

    public class PayActivity : BaseActivity<PayContext>
    {
        protected override Task Execute(PayContext data)
        {
            LogHelper.Info("发起支付处理");
            return Task.CompletedTask;
        }
    }

    public class StockConnector : BaseConnector<PayContext, StockContext>
    {
        protected override StockContext Convert(PayContext inContextData)
        {
            return new StockContext() { id = inContextData.id };
        }
    }

    public class StockActivity : BaseActivity<StockContext>
    {
        protected override Task Execute(StockContext data)
        {
            LogHelper.Info("库存保存");
            return Task.CompletedTask;
        }
    }
}
