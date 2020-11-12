
#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：OSS.EventTask - 配置元数据基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       创建时间： 2019-04-07
*       
*****************************************************************************/

#endregion


using System;
using System.Threading.Tasks;

namespace OSS.EventTask.MetaMos
{
    public class BaseMeta<TMetaType>//: IMeta<TMetaType>
        where TMetaType : class
    {
        private TMetaType _meta { get; set; }

        public BaseMeta()
        {
        }

        public BaseMeta(TMetaType meta)
        {
            _meta = meta;
        }

        internal async Task<TMetaType> GetMeta()
        {
            var meta = await GetCustomMeta();
            if (meta!=null)
            {
                return meta;
            }

            if (_meta==null)
            {
                throw new NullReferenceException("获取Meta信息为空，请在构造函数传入或重写GetCustomMeta方法");
            }
            return _meta;
        }

        protected virtual Task<TMetaType> GetCustomMeta()
        {
            return Task.FromResult<TMetaType>(null);
        }
    }
}