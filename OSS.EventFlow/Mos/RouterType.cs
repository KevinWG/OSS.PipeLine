namespace OSS.EventFlow.Mos
{
    public enum RouterType
    {
        /// <summary>
        ///  串行
        /// </summary>
        Serial=10,

        /// <summary>
        /// 并行
        /// </summary>
        Branch=20,

        /// <summary>
        /// 循环后串行
        /// </summary>
        SerialAfterCycle=30,

        /// <summary>
        ///   循环后并行
        /// </summary>
        BranchAfterCycle=40
    }
}
