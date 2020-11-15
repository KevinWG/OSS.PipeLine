



using System.Threading.Tasks;

namespace OSS.EventFlow.Gateway
{
    public interface IDataEndpoint<in DataType>
    {
        /// <summary>
        ///  消息出端口的调用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Pop(DataType data);
    }

    public interface IFlowTunnel<in DataType> : IDataEndpoint<DataType>
    {
        /// <summary>
        ///  消息进入
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Post(DataType data);
    }
}
