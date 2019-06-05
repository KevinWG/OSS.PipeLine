using OSS.EventFlow.Agent;
using OSS.EventFlow.Gateway;

namespace OSS.EventFlow
{
  
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
        
        /// <summary>
        ///  异常代理
        /// </summary>
        public BaseAgent UnusualAgent { get; set; }
        
        protected void AddAgent(BaseAgent agent, BaseGateway gateway)
        {

        }

    }
}
