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

        ///// <summary>
        /////  通用配置实现基类
        ///// </summary>
        //public abstract class BaseMetaImpl<TMetaType>:IMetaProvider<TMetaType>
        //    where TMetaType : class
        //{
        //    private readonly IMetaProvider<TMetaType> _metaProvider;

        //    /// <summary>
        //    /// 构造函数
        //    /// </summary>
        //    protected BaseMetaImpl()
        //    {
        //    }

        //    /// <summary>
        //    /// 构造函数
        //    /// </summary>
        //    /// <param name="metaProvider">动态的配置提供者</param>
        //    protected BaseMetaImpl(IMetaProvider<TMetaType> metaProvider)
        //    {
        //        _metaProvider = metaProvider;
        //    }

        //    /// <summary>
        //    /// 获取当前元配置信息
        //    /// </summary>
        //    /// <returns></returns>
        //    public Task<TMetaType> GetMeta()
        //    {
        //        if (_metaProvider != null)
        //        {
        //            return _metaProvider.GetMeta();
        //        }

        //        var defaultMeta = GetDefaultMeta();
        //        if (defaultMeta == null)
        //        {
        //            throw new ArgumentNullException("未发现任何配置信息,你可在构造函数中注入IMetaProvider实现，也可以重写GetDefaultMeta方法来完成配置的提供!");
        //        }
        //        return Task.FromResult(defaultMeta);
        //    }


        //    #region 扩展虚方法

        //    /// <summary>
        //    /// 获取默认配置信息（如：静态固定的配置信息）
        //    /// </summary>
        //    /// <returns></returns>
        //    protected virtual TMetaType GetDefaultMeta()
        //    {
        //        return null;
        //    }
        //    #endregion
        //}
    }