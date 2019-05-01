﻿namespace OSS.EventTask.Mos
{
    public class TaskReqData<TReq, TDomain> : TaskReqData<TReq>
    {
        public TaskReqData()
        {
        }

        public TaskReqData(TReq req, TDomain domainData):base(req)
        {
            domain_data = domainData;
        }

        /// <summary>
        ///   核心流数据
        /// </summary>
        public TDomain domain_data { get; set; }
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
