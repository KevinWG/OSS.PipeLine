using OSS.EventFlow.Agent;
using OSS.EventFlow.Gateway;

namespace OSS.EventFlow
{
    //  todo  设置启动和结束默认agent，  异常agent
    //  todo  递归执行获取路径地图
    public  abstract partial class BaseFlow
    {
        /// <summary>
        ///  流程起始
        /// </summary>
        public BaseGateway Start { get; set; }
        
        /// <summary>
        /// 流程结束代理
        /// </summary>
        public BaseGateway Stop { get; set; }





        protected void AddAgent(BaseAgent agent, BaseGateway gateway)
        {

        }

    }
}
