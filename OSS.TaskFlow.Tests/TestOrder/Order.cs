namespace OSS.TaskFlow.Tests.TestOrder
{
    public class Order
    {
        public void AddOrder(OrderInfo order)
        {
            
        }
    }

    public class OrderInfo
    {
        public string order_alias { get; set; }

        public decimal price { get; set; }

        public int id { get; set; }

        /// <summary>
        ///   
        /// </summary>
        public int status { get; set; }
    }
}
