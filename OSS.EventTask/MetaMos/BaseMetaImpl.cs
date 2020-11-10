namespace OSS.EventTask.MetaMos
{
    /// <summary>
    /// 元配置信息提供接口定义
    /// </summary>
    /// <typeparam name="TMetaType">元配置信息类型</typeparam>
    public interface IMeta<TMetaType>
        where TMetaType : class
    {

        TMetaType Meta { get; set; }
    }

    public class BaseMeta<TMetaType>: IMeta<TMetaType>
        where TMetaType : class
    {
        public TMetaType Meta { get; set; }

        public BaseMeta()
        {
        }

        public BaseMeta(TMetaType meta)
        {
            Meta = meta;
        }

    }
}