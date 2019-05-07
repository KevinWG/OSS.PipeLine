namespace OSS.EventTask.Mos
{
    /// <summary>
    ///  请求数据
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TDomain"></typeparam>
    public class ExcuteReq<TDomain,TReq> : ExcuteReq<TReq>
    {
        /// <summary>
        ///   核心流数据
        /// </summary>
        public TDomain domain_data { get; set; }
    }
    
    /// <summary>
    ///   请求数据
    /// </summary>
    /// <typeparam name="TReq"></typeparam>
    public class ExcuteReq<TReq> : ExcuteReq
    {
        /// <summary>
        ///   执行请求内容主体
        /// </summary>
        public TReq req_data { get; set; }
    }
    
    public class ExcuteReq
    {
        public string exc_id { get; set; }
    }

    public class RunCondition
    {
        /// <summary>
        ///  执行总次数
        /// </summary>
        public int exced_times { get; internal set; }

        /// <summary>
        ///  间隔执行次数
        /// </summary>
        public int interval_times { get; internal set; }

        /// <summary>
        ///  上次执行时间
        /// </summary>
        public long last_Processdate { get; set; }
    }
}
