using OSS.EventFlow.Gateway;

namespace OSS.EventFlow.Agent
{
    public static class EventAgentExtention
    {
        /// <summary>
        ///  设置顺序节点
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="gateway"></param>
        public static void WithGateway(this BaseAgent agent,BaseGateway gateway )
        {
            agent.Gateway = gateway;
        }
    }

}
