namespace OSS.Pipeline
{
    /// <summary>
    ///  空上下文
    /// </summary>
    public struct EmptyContext
    {
        /// <summary>
        ///  默认空上下文
        /// </summary>
        public static EmptyContext Default { get; } 

        static EmptyContext() {
            Default = new EmptyContext();
        }
    }
}