namespace OSS.EventTask.Mos
{
    public interface IExecuteData
    {
        string exe_id { get; set; }
    }



    ///// <summary>
    /////  请求数据
    ///// </summary>
    ///// <typeparam name="TReq"></typeparam>
    ///// <typeparam name="TDomain"></typeparam>
    //public class ExecuteData<TDomain,TReq> : ExecuteData<TReq>
    //{
    //    public ExecuteData()
    //    {
    //    }

    //    public ExecuteData(TDomain doaminData, TReq reqData)
    //    {
    //        domain_data = doaminData;
    //        req_data = reqData;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string domain_id { get; set; }

    //    /// <summary>
    //    ///   核心流数据
    //    /// </summary>
    //    public TDomain domain_data { get; set; }
    //}

    ///// <summary>
    /////   请求数据
    ///// </summary>
    ///// <typeparam name="TReq"></typeparam>
    //public class ExecuteData<TReq> : ExecuteData
    //{
    //    public ExecuteData()
    //    {
    //    }

    //    public ExecuteData(TReq reqData)
    //    {
    //        req_data = reqData;
    //    }

    //    /// <summary>
    //    ///   执行请求内容主体
    //    /// </summary>
    //    public TReq req_data { get; set; }
    //}

    //public class ExecuteData
    //{
    //    public string exe_id { get; set; }
    //}


}
