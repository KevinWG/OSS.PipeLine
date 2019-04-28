namespace OSS.TaskFlow.Tasks.Mos
{
    public class TaskReqData<TReq, TFlowData> : TaskReqData<TReq>
    {
        public TaskReqData()
        {
        }

        public TaskReqData(TReq req, TFlowData flowData):base(req)
        {
            flow_data = flowData;
        }

        /// <summary>
        ///   核心流数据
        /// </summary>
        public TFlowData flow_data { get; set; }
    }

    /// <summary>
    ///   请求数据
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    public class TaskReqData<TReq> : TaskReqData
    {
        public TaskReqData()
        {
        }

        public TaskReqData(TReq req)
        {
            req_data = req;
        }

        /// <summary>
        ///   执行请求内容主体
        /// </summary>
        public TReq req_data { get; set; }
    }

    public abstract class TaskReqData
    {
    }
}
